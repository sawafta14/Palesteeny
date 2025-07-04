document.addEventListener("DOMContentLoaded", () => {
    // ========== Assistant Section ==========
    const genderSelect = document.getElementById("gender");
    const colorImages = document.querySelectorAll(".assistant-colors");
    const assistantImg = document.querySelector(".assistant-img");
    const assistantName = document.getElementById("assistantName");

    let selectedGender = genderSelect.value;
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

    genderSelect.addEventListener("change", (e) => {
        selectedGender = e.target.value;
        updateAssistantImage();
        updateAssistantName();
    });

    colorImages.forEach(img => {
        img.addEventListener("click", () => {
            const src = img.src.toLowerCase();
            const colorMatch = src.match(/\/([a-z]+)-color\.png$/);
            if (colorMatch) {
                selectedColor = colorMatch[1].charAt(0).toUpperCase() + colorMatch[1].slice(1);
                updateAssistantImage();
                updateColorHighlight(img);
            }
        });
    });

    updateAssistantImage();
    updateColorHighlight(colorImages[0]);
    updateAssistantName();

    // ========== Profile Edit Dialog ==========
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
                inputGrade.value = user.grade || "";
                inputSem.value = user.semester || "";
                dialog.classList.remove("hidden");
                errorMsg.textContent = "";
            });
    });

    cancelBtn.addEventListener("click", () => {
        dialog.classList.add("hidden");
    });

    applyBtn.addEventListener("click", () => {
        const firstName = inputFirstName.value.trim();
        const lastName = inputLastName.value.trim();
        const grade = inputGrade.value;
        const semester = inputSem.value;
        const age = calculateAge(grade);

        if (!firstName || !lastName || !grade || !semester) {
            errorMsg.textContent = "الرجاء تعبئة جميع الحقول.";
            return;
        }

        fetch("/Profile/UpdateUserInfo", {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify({ firstName, lastName, grade, semester, age })
        })
            .then(response => {
                if (response.ok) {
                    nameField.innerHTML = `<strong>اسمي:</strong> ${firstName} ${lastName}`;
                    gradeField.innerHTML = `<strong>مرحلتي الدراسية:</strong> ${grade}`;
                    semField.innerHTML = `<strong>الفصل الدراسي:</strong> ${semester}`;
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
