document.addEventListener("DOMContentLoaded", () => {
    // ========== Assistant Section ==========
    const genderSelect = document.getElementById("gender");
    const colorImages = document.querySelectorAll(".assistant-colors");
    const assistantImg = document.querySelector(".assistant-img");
    const assistantName = document.getElementById("assistantName");

    let selectedGender = "male";
    let selectedColor = "Blue";

    function getImageName(color, gender) {
        const suffix = gender === "male" ? "Teeny" : "Teena";
        return `/images/${suffix}${color}.svg`;
    }

    function updateAssistantImage() {
        assistantImg.src = getImageName(selectedColor, selectedGender);
    }

    function updateAssistantName() {
        assistantName.textContent = selectedGender === "male" ? "تيني" : "تينا";
    }

    function updateColorHighlight(selectedElement) {
        colorImages.forEach(img => img.classList.remove("selected-color"));
        selectedElement.classList.add("selected-color");
    }

    function sendAssistantPreference() {
        fetch("/Profile/UpdateAssistantPreference", {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify({
                gender: selectedGender,
                color: selectedColor
            })
        }).then(res => {
            if (!res.ok) console.error("فشل حفظ تفضيل المساعد");
        });
    }

    genderSelect.addEventListener("change", (e) => {
        selectedGender = e.target.value;
        updateAssistantImage();
        updateAssistantName();
        sendAssistantPreference();
    });

    colorImages.forEach(img => {
        img.addEventListener("click", () => {
            const src = img.src.toLowerCase();
            const colorMatch = src.match(/\/([a-z]+)-color\.png$/);
            if (colorMatch) {
                selectedColor = colorMatch[1].charAt(0).toUpperCase() + colorMatch[1].slice(1);
                updateAssistantImage();
                updateColorHighlight(img);
                sendAssistantPreference();
            }
        });
    });

    // Fetch user info initially to set defaults
    fetch("/Profile/GetUserInfo")
        .then(res => res.json())
        .then(user => {
            if (user.aiGender) {
                selectedGender = user.aiGender;
                genderSelect.value = selectedGender;
            }
            if (user.aiColor) {
                selectedColor = user.aiColor;
                colorImages.forEach(img => {
                    if (img.src.toLowerCase().includes(user.aiColor.toLowerCase())) {
                        updateColorHighlight(img);
                    }
                });
            }
            updateAssistantImage();
            updateAssistantName();

            // ✅ ADD THIS to show grade & semester on page load
            if (user.semester?.gradeName) {
                gradeField.innerHTML = `<strong>مرحلتي الدراسية:</strong> ${user.semester.gradeName}`;
            }
            if (user.semester?.semesterName) {
                semField.innerHTML = `<strong>الفصل الدراسي:</strong> ${user.semester.semesterName}`;
            }
        });

    // ========== Profile Edit Dialog ==========
    let semesters = [];



    loadSemestersFromDB();

    const editBtn = document.querySelector(".edit-btn");
    const dialog = document.getElementById("editDialog");
    const applyBtn = document.getElementById("applyBtn");
    const cancelBtn = document.getElementById("cancelBtn");
    const errorMsg = document.getElementById("dialogError");

    const nameField = document.querySelector(".profile-info p:nth-child(1)");
    const gradeField = document.querySelector(".profile-info p:nth-child(2)");
    const semField = document.querySelector(".profile-info p:nth-child(3)");

    const inputFirstName = document.getElementById("editFirstName");
    const inputLastName = document.getElementById("editLastName");
    const inputGrade = document.getElementById("editGrade");
    const inputSem = document.getElementById("editSem");

    function loadSemestersFromDB() {
        fetch("/Profile/GetSemesters")
            .then(res => res.json())
            .then(data => {
                semesters = data;

                const gradeSet = new Set(data.map(s => s.gradeName));
                inputGrade.innerHTML = '<option value="">اختر المرحلة</option>';
                gradeSet.forEach(grade => {
                    if (grade) {
                        const opt = document.createElement("option");
                        opt.value = grade;
                        opt.textContent = grade;
                        inputGrade.appendChild(opt);
                    }
                });
            });
    }
    inputGrade.addEventListener("change", () => {
        const selectedGrade = inputGrade.value;

        inputSem.innerHTML = '<option value="">اختر الفصل</option>'; // reset

        semesters
            .filter(s => s.gradeName === selectedGrade)
            .forEach(s => {
                const opt = document.createElement("option");
                opt.value = s.semesterName;
                opt.textContent = s.semesterName;
                inputSem.appendChild(opt);
            });
    });
    function calculateAge(grade) {
        const map = {
            "الأول ابتدائي": 6,
            "الثاني ابتدائي": 7,
            "الثالث ابتدائي": 8,
            "الرابع ابتدائي": 9
        };
        return map[grade] || null;
    }

    editBtn.addEventListener("click", () => {
        fetch("/Profile/GetUserInfo")
            .then(res => res.json())
            .then(user => {
                inputFirstName.value = user.firstName || "";
                inputLastName.value = user.lastName || "";
                inputGrade.value = user.semester?.gradeName || "";
                inputGrade.dispatchEvent(new Event("change")); // 🟡 trigger semester population

                // Wait a tiny bit so the semesters populate before selecting the correct one
                setTimeout(() => {
                    inputSem.value = user.semester?.semesterName || "";
                }, 100);

                dialog.classList.remove("hidden");
                errorMsg.textContent = "";
            });
    });

    cancelBtn.addEventListener("click", () => {
        dialog.classList.add("hidden");
    });

    applyBtn.addEventListener("click", () => {
        const grade = inputGrade.value;
        const sem = inputSem.value;
        const firstName = inputFirstName.value.trim();
        const lastName = inputLastName.value.trim();

        if (!firstName || !lastName || !grade || !sem) {
            errorMsg.textContent = "الرجاء تعبئة جميع الحقول.";
            return;
        }

        // 🔍 Find SemesterId
        const selectedSemester = semesters.find(s =>
            s.gradeName === grade && s.semesterName === sem
        );
        if (!selectedSemester) {
            errorMsg.textContent = "الصف والفصل المحددان غير موجودين.";
            return;
        }

        const semesterId = selectedSemester.id;

        fetch("/Profile/UpdateProfile", {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify({ firstName, lastName, semesterId })
        })
            .then(response => {
                if (response.ok) {
                    nameField.innerHTML = `<strong>اسمي:</strong> ${firstName} ${lastName}`;
                    gradeField.innerHTML = `<strong>مرحلتي الدراسية:</strong> ${grade}`;
                    semField.innerHTML = `<strong>الفصل الدراسي:</strong> ${sem}`;
                    dialog.classList.add("hidden");
                } else {
                    errorMsg.textContent = "حدث خطأ أثناء التحديث.";
                }
            });

    });

    // ========== Profile Image Upload & Lightbox ==========
    const overlay = document.getElementById("overlay");
    const fileInput = document.getElementById("imageUpload");
    const profileImg = document.getElementById("profileImg");
    const imageMenu = document.getElementById("imageMenu");
    const lightbox = document.getElementById("lightbox");
    const lightboxImg = document.getElementById("lightboxImg");
    const closeLightboxBtn = document.getElementById("closeLightbox");

    overlay.addEventListener("click", () => {
        imageMenu.classList.remove("hidden");
    });

    document.addEventListener("click", (e) => {
        if (!imageMenu.contains(e.target) && !overlay.contains(e.target)) {
            imageMenu.classList.add("hidden");
        }
    });

    imageMenu.addEventListener("click", (e) => {
        const action = e.target.getAttribute("data-action");
        if (action === "upload") {
            fileInput.click();
        } else if (action === "view") {
            lightboxImg.src = profileImg.src;
            lightbox.classList.remove("hidden");
            lightbox.setAttribute("aria-hidden", "false");
        }
        imageMenu.classList.add("hidden");
    });

    closeLightboxBtn.addEventListener("click", () => {
        lightbox.classList.add("hidden");
        lightbox.setAttribute("aria-hidden", "true");
        lightboxImg.src = "";
    });

    lightbox.addEventListener("click", (e) => {
        if (e.target === lightbox) {
            lightbox.classList.add("hidden");
            lightbox.setAttribute("aria-hidden", "true");
            lightboxImg.src = "";
        }
    });

    fileInput.addEventListener("change", (e) => {
        const file = e.target.files[0];
        if (file && file.type.startsWith("image/")) {
            const reader = new FileReader();
            reader.onload = (event) => {
                profileImg.src = event.target.result;
            };
            reader.readAsDataURL(file);

            const formData = new FormData();
            formData.append("image", file);

            fetch("/Profile/UploadProfileImage", {
                method: "POST",
                body: formData
            })
                .then(res => res.json())
                .then(data => {
                    if (data.imageUrl) {
                        profileImg.src = data.imageUrl;
                    }
                })
                .catch(err => console.error("Upload failed", err));
        }
    });
});
