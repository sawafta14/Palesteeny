document.addEventListener("DOMContentLoaded", function () {
    const container = document.getElementById("exercise-container");
    const checkBtn = document.getElementById("check-answers");
    const prevBtn = document.getElementById("prev-btn");
    const nextBtn = document.getElementById("next-btn");

    const params = new URLSearchParams(window.location.search);
    const lessonVal = params.get("lesson");
    const dataUrl = `/api/ExerciseApi/lesson/${lessonVal}`;
    const userSemester = params.get("sem");
    const bookIdFromRazorView = params.get("bookId");
    const totalLessons = parseInt(params.get("totalLessons"), 10) || 0;
    const lessonDropdown = document.getElementById('lessonDropdown');


    let currentExercise = null;
    const groupsPerPage = 2;
    let currentPageIndex = 0;
    const studentAnswers = {};
    let answersChecked = false;

    async function loadExercises() {
        try {
            const res = await fetch(dataUrl);
            if (!res.ok) throw new Error(`HTTP error! status: ${res.status}`);
            currentExercise = await res.json();

            if (!Array.isArray(currentExercise) || currentExercise.length === 0) {
                container.innerHTML = "<p>لا توجد تمارين لهذا الدرس حتى الآن.</p>";
                checkBtn.style.display = "none";
                prevBtn.disabled = true;
                nextBtn.disabled = true;
                return;
            }

            renderCurrentPage();
        } catch (err) {
            container.innerHTML = "<p>لا توجد تمارين لهذا الدرس حتى الآن.</p>";
            checkBtn.style.display = "none";
            prevBtn.disabled = true;
            nextBtn.disabled = true;
            console.error("فشل تحميل التمرين:", err);
        }
    }

    function renderCurrentPage() {
        const start = currentPageIndex * groupsPerPage;
        const end = start + groupsPerPage;
        const groupsToRender = currentExercise.slice(start, end);

        container.innerHTML = "";
        const pageWrapper = document.createElement("div");
        pageWrapper.className = "page-wrapper";

        groupsToRender.forEach((group, groupIndex) => {
            const block = document.createElement("div");
            block.className = `question-block ${groupIndex === 0 ? "block-top" : "block-bottom"}`;

            const questionHeader = document.createElement("h2");
            questionHeader.className = "question-header";
            questionHeader.textContent = `نشاط ${start + groupIndex + 1}`;
            block.appendChild(questionHeader);

            const inner = document.createElement("div");
            inner.className = "inner-block";

            const left = document.createElement("div");
            left.className = "left-side";

            if (group.image) {
                const questionDisplay = document.createElement("div");
                questionDisplay.className = group.questionOverlay ? "question-background" : "question-image-wrapper";

                const img = document.createElement("img");
                img.src = group.image;
                img.alt = "صورة";
                questionDisplay.appendChild(img);

                const text = document.createElement("div");
                text.className = group.questionOverlay ? "question-text" : "question-text-under";
                text.innerHTML = group.question || "";
                questionDisplay.appendChild(text);

                left.appendChild(questionDisplay);
            } else {
                const questionDisplay = document.createElement("div");
                questionDisplay.className = "plain-question-text";
                questionDisplay.innerHTML = group.question || "";
                left.appendChild(questionDisplay);
            }

            const right = document.createElement("div");
            right.className = "right-side";

            (group.questions || []).forEach(q => {
                const answerBlock = document.createElement("div");
                answerBlock.className = "answer-block";

                if (group.type === "true_false") {
                    (group.questions || []).forEach(q => {
                        (q.options || []).forEach(opt => {
                            const row = document.createElement("div");
                            row.className = "tf-row";

                            if (opt.image) {
                                const optImg = document.createElement("img");
                                optImg.src = opt.image;
                                optImg.alt = "خيار";
                                optImg.className = "tf-image";
                                row.appendChild(optImg);
                            } else if (opt.text) {
                                const optText = document.createElement("div");
                                optText.textContent = opt.text;
                                optText.className = "tf-text";
                                row.appendChild(optText);
                            }

                            const btnGroup = document.createElement("div");
                            btnGroup.className = "tf-btns";

                            const trueBtn = document.createElement("button");
                            trueBtn.className = "tf-btn";
                            trueBtn.textContent = "✔";
                            trueBtn.dataset.name = `${q.id}_${opt.id}`;
                            trueBtn.dataset.value = "true";

                            const falseBtn = document.createElement("button");
                            falseBtn.className = "tf-btn";
                            falseBtn.textContent = "✘";
                            falseBtn.dataset.name = `${q.id}_${opt.id}`;
                            falseBtn.dataset.value = "false";

                            btnGroup.appendChild(trueBtn);
                            btnGroup.appendChild(falseBtn);

                            row.appendChild(btnGroup);
                            answerBlock.appendChild(row);
                        });
                    });
                } else if (group.type === "multiple_choice") {
                    (group.questions || []).forEach(q => {
                        (q.options || []).forEach(opt => {
                            const btn = document.createElement("button");
                            btn.className = "choice-btn";
                            btn.dataset.name = q.id;
                            btn.dataset.value = opt.text || "";

                            btn.innerHTML = opt.image
                                ? `<img src="${opt.image}" alt="خيار" style="height:50px;"><br>${opt.text || ""}`
                                : opt.text || "";

                            answerBlock.appendChild(btn);
                        });
                    });
                } else if (group.type === "drag_drop") {
                    (group.questions || []).forEach(q => {
                        const matches = q.matches || [];
                        const optionsHTML = matches.map(item =>
                            item.optionImageUrl
                                ? `<div class="drag-item" draggable="true" data-val="${item.optionText}">
                    <img src="${item.optionImageUrl}" alt="خيار" height="50"><br>${item.optionText}
                   </div>`
                                : `<div class="drag-item" draggable="true" data-val="${item.optionText}">${item.optionText}</div>`
                        ).join('');

                        const targetsHTML = matches.map(item =>
                            item.matchImageUrl
                                ? `<div class="drop-zone" data-match="${item.matchLabel}">
                    <img src="${item.matchImageUrl}" alt="هدف" height="80"><br>${item.matchLabel}
                   </div>`
                                : `<div class="drop-zone" data-match="${item.matchLabel}">${item.matchLabel}</div>`
                        ).join('');

                        answerBlock.innerHTML += `<div>${optionsHTML}</div><div style="margin-top:10px;">${targetsHTML}</div>`;
                    });
                }

                right.appendChild(answerBlock);


            });

            inner.appendChild(left);
            inner.appendChild(right);
            block.appendChild(inner);
            pageWrapper.appendChild(block);
        });

        container.appendChild(pageWrapper);
        updateNavButtons();
        initDragDrop();
        restoreSavedAnswers();
        bindAnswerEvents();
        if (answersChecked) {
            applyEvaluationStyles();
        } 
        
        const totalPages = Math.ceil(currentExercise.length / groupsPerPage);
        checkBtn.style.display = (currentPageIndex === totalPages - 1) ? "block" : "none";
    }
    function restoreSavedAnswers() {
        // Restore multiple_choice selections
        document.querySelectorAll(".choice-btn").forEach(btn => {
            const name = btn.dataset.name;
            if (studentAnswers[name] === btn.dataset.value) {
                btn.classList.add("selected");
            }
        });

        // Restore true_false selections
        document.querySelectorAll(".tf-btn").forEach(btn => {
            const name = btn.dataset.name;
            if (studentAnswers[name] === btn.dataset.value) {
                btn.classList.add("selected");
            }
        });

        // Restore drag & drop answers
        document.querySelectorAll(".drop-zone").forEach(zone => {
            const matchLabel = zone.dataset.match;
            const groupKey = Object.keys(studentAnswers).find(k => Array.isArray(studentAnswers[k]));

            if (groupKey && studentAnswers[groupKey]) {
                const savedItem = studentAnswers[groupKey].find(i => i.target === matchLabel);
                if (savedItem && savedItem.value) {
                    const dropped = document.createElement("div");
                    dropped.className = "dropped-val";
                    dropped.textContent = savedItem.value;
                    dropped.setAttribute("draggable", true);
                    dropped.style.marginTop = "8px";
                    dropped.style.backgroundColor = "#d4ffd4";
                    dropped.style.borderRadius = "8px";
                    dropped.style.padding = "5px";
                    dropped.style.cursor = "grab";

                    dropped.addEventListener("dragstart", ev => {
                        ev.dataTransfer.setData("text/plain", savedItem.value);
                        ev.dataTransfer.effectAllowed = "move";
                        setTimeout(() => dropped.remove(), 0);
                        delete zone.dataset.current;
                    });

                    zone.appendChild(dropped);
                    zone.dataset.current = savedItem.value;
                }
            }
        });
    }
    function applyEvaluationStyles() {
        currentExercise.forEach(group => {
            (group.questions || []).forEach(q => {
                if (group.type === "true_false") {
                    (q.options || []).forEach(opt => {
                        const selector = `[data-name="${q.id}_${opt.id}"]`;
                        const buttons = document.querySelectorAll(selector);
                        const userAnswer = studentAnswers[`${q.id}_${opt.id}`];
                        buttons.forEach(btn => {
                            btn.classList.remove("correct", "incorrect");
                            const correctAnswer = opt.isCorrectAnswer === true ? "true" : "false";

                            if (userAnswer === btn.dataset.value) {
                                if (userAnswer === correctAnswer) {
                                    btn.classList.add("correct");
                                } else {
                                    btn.classList.add("incorrect");
                                }
                            }

                        });
                    });
                } else if (group.type === "multiple_choice") {
                    const correctOption = q.options.find(opt => opt.isCorrect === true);
                    const userAnswer = studentAnswers[q.id];
                    const buttons = document.querySelectorAll(`.choice-btn[data-name="${q.id}"]`);
                    buttons.forEach(btn => {
                        btn.classList.remove("correct", "incorrect");
                        if (btn.dataset.value === userAnswer) {
                            btn.classList.add(userAnswer === (correctOption?.text || "") ? "correct" : "incorrect");
                        }
                    });
                } else if (group.type === "drag_drop") {
                    // Drag-drop evaluation UI depends on your style, implement if needed
                }
            });
        });
    }

    function bindAnswerEvents() {
        document.querySelectorAll(".choice-btn").forEach(btn => {
            btn.addEventListener("click", function () {
                const name = this.dataset.name;
                document.querySelectorAll(`.choice-btn[data-name="${name}"]`).forEach(b => {
                    b.classList.remove("selected", "correct", "incorrect");
                });
                this.classList.add("selected");
                studentAnswers[name] = this.dataset.value;
            });
        });

        document.querySelectorAll(".tf-btn").forEach(btn => {
            btn.addEventListener("click", function () {
                const name = this.dataset.name;
                document.querySelectorAll(`.tf-btn[data-name="${name}"]`).forEach(b => {
                    b.classList.remove("selected", "correct", "incorrect");
                });
                this.classList.add("selected");
                studentAnswers[name] = this.dataset.value;
            });
        });

        // ✅ Resize font for long buttons
        document.querySelectorAll(".tf-btn").forEach(btn => {
            const textLength = btn.textContent.trim().length;
            if (textLength > 5 || btn.offsetWidth < btn.scrollWidth) {
                btn.classList.add("auto-resize");
            }
        });
    }

    function saveAllAnswers() {
        for (let page = 0; page < Math.ceil(currentExercise.length / groupsPerPage); page++) {
            const start = page * groupsPerPage;
            const end = start + groupsPerPage;
            const groupsToSave = currentExercise.slice(start, end);

            groupsToSave.forEach(group => {
                const questions = Array.isArray(group.questions) && group.questions.length > 0
                    ? group.questions
                    : [group];

                questions.forEach(q => {
                    let answer = null;
                    if (group.type === "multiple_choice") {
                        const selected = document.querySelector(`.choice-btn[data-name="${q.id}"].selected`);
                        answer = selected?.dataset.value;
                    } else if (group.type === "true_false") {
                        (group.questions || []).forEach(q => {
                            (q.options || []).forEach(opt => {
                                const selected = document.querySelector(`.tf-btn[data-name="${q.id}_${opt.id}"].selected`);
                                studentAnswers[`${q.id}_${opt.id}`] = selected?.dataset.value;
                            });
                        });
                        return; // already saved all sub-answers
                    } else if (group.type === "drag_drop") {
                        const zones = document.querySelectorAll(`.drop-zone`);
                        answer = Array.from(zones).map(z => ({
                            target: z.dataset.match,
                            value: z.dataset.current || ""
                        }));
                    }
                    studentAnswers[q.id] = answer;
                });
            });
        }
    }


    function initDragDrop() {
        const drags = document.querySelectorAll(".drag-item");
        const drops = document.querySelectorAll(".drop-zone");

        drags.forEach(el => {
            el.setAttribute("draggable", true);
            el.addEventListener("dragstart", e => {
                e.dataTransfer.setData("text/plain", el.dataset.val);
                e.dataTransfer.effectAllowed = "move";
            });
        });

        drops.forEach(zone => {
            zone.addEventListener("dragover", e => e.preventDefault());
            zone.addEventListener("drop", e => {
                e.preventDefault();
                const val = e.dataTransfer.getData("text/plain");

                if (zone.querySelector(".dropped-val")) return;

                const dropped = document.createElement("div");
                dropped.className = "dropped-val";
                dropped.textContent = val;
                dropped.setAttribute("draggable", true);
                dropped.style.marginTop = "8px";
                dropped.style.backgroundColor = "#d4ffd4";
                dropped.style.borderRadius = "8px";
                dropped.style.padding = "5px";
                dropped.style.cursor = "grab";

                dropped.addEventListener("dragstart", ev => {
                    ev.dataTransfer.setData("text/plain", val);
                    ev.dataTransfer.effectAllowed = "move";
                    setTimeout(() => dropped.remove(), 0);
                    delete zone.dataset.current;
                });

                zone.appendChild(dropped);
                zone.dataset.current = val;
            });
        });

        const dragArea = document.querySelectorAll(".drag-item")[0]?.parentElement;
        if (dragArea) {
            dragArea.addEventListener("dragover", e => e.preventDefault());
            dragArea.addEventListener("drop", e => e.preventDefault());
        }
    }

    function updateNavButtons() {
        const totalPages = Math.ceil(currentExercise.length / groupsPerPage);
        prevBtn.disabled = currentPageIndex === 0;
        nextBtn.disabled = currentPageIndex >= totalPages - 1;
    }

    prevBtn.addEventListener("click", () => {
        saveCurrentPageAnswers();
        if (currentPageIndex > 0) {
            currentPageIndex--;
            renderCurrentPage();
        }
    });

    nextBtn.addEventListener("click", () => {
        saveCurrentPageAnswers();
        const totalPages = Math.ceil(currentExercise.length / groupsPerPage);
        if (currentPageIndex < totalPages - 1) {
            currentPageIndex++;
            renderCurrentPage();
        }
    });
    async function submitAnswersToServer(payload) {
        try {
            const res = await fetch("/api/ExerciseApi/SubmitAnswers", {
                method: "POST",
                headers: {
                    "Content-Type": "application/json"
                },
                body: JSON.stringify(payload)
            });
            if (!res.ok) throw new Error(`HTTP error! status: ${res.status}`);

            const result = await res.json();
            console.log(result.message || "تم الحفظ");
        } catch (err) {
            console.error("خطأ في إرسال الإجابات:", err);
        }
    }

    function evaluateAll() {
        let allCorrect = true;

        currentExercise.forEach(group => {
            (group.questions || []).forEach(q => {
                if (group.type === "true_false") {
                    (q.options || []).forEach(opt => {
                        const correctAnswer = opt.isCorrectAnswer === true ? "true" : "false";
                        const userAnswer = (studentAnswers[`${q.id}_${opt.id}`] || "").toLowerCase();

                        if (userAnswer !== correctAnswer) {
                            console.log(`❌ Incorrect: Q${q.id}, Option${opt.id} — Expected: ${correctAnswer}, Got: ${userAnswer}`);
                            allCorrect = false;
                        } else {
                            console.log(`✅ Correct: Q${q.id}, Option${opt.id}`);
                        }
                    });
                }

                else if (group.type === "multiple_choice") {
                    const userAnswer = studentAnswers[q.id];
                    const correctOption = q.options.find(opt => opt.isCorrect === true);
                    if (!userAnswer || userAnswer !== (correctOption?.text || "")) {
                        console.log(`False multiple choice at question ${q.id}: userAnswer=${userAnswer}, correct=${correctOption?.text}`);
                        allCorrect = false;
                    }
                } else if (group.type === "drag_drop") {
                    const userAnswer = studentAnswers[q.id] || [];
                    q.matches.forEach(match => {
                        const userMatch = userAnswer.find(u => u.target === match.matchLabel);
                        if (!userMatch || userMatch.value !== match.optionText) {
                            console.log(`False drag-drop at question ${q.id}, match ${match.matchLabel}`);
                            allCorrect = false;
                        }
                    });
                }
            });
        });

        console.log("evaluateAll result:", allCorrect);
        return allCorrect;
    }


    function saveCurrentPageAnswers() {
        const start = currentPageIndex * groupsPerPage;
        const end = start + groupsPerPage;
        const groupsToSave = currentExercise.slice(start, end);

        groupsToSave.forEach(group => {
            const questions = Array.isArray(group.questions) && group.questions.length > 0
                ? group.questions
                : [group];

            questions.forEach(q => {
                let answer = null;
                if (group.type === "multiple_choice") {
                    const selected = document.querySelector(`.choice-btn[data-name="${q.id}"].selected`);
                    answer = selected?.dataset.value;
                } else if (group.type === "true_false") {
                    (group.questions || []).forEach(q => {
                        (q.options || []).forEach(opt => {
                            const selected = document.querySelector(`.tf-btn[data-name="${q.id}_${opt.id}"].selected`);
                            studentAnswers[`${q.id}_${opt.id}`] = selected?.dataset.value;
                        });
                    });
                    return; // sub-answers saved
                } else if (group.type === "drag_drop") {
                    const zones = document.querySelectorAll(`.drop-zone`);
                    answer = Array.from(zones).map(z => ({
                        target: z.dataset.match,
                        value: z.dataset.current || ""
                    }));
                }
                studentAnswers[q.id] = answer;
            });
        });
    }

    function syncStudentAnswersFromServer() {
        // Clear existing answers
        for (const key in studentAnswers) {
            delete studentAnswers[key];
        }

        currentExercise.forEach(group => {
            (group.questions || []).forEach(q => {
                if (group.type === "true_false") {
                    (q.options || []).forEach(opt => {
                        if (opt.userAnswer !== undefined && opt.userAnswer !== null) {
                            studentAnswers[`${q.id}_${opt.id}`] = opt.userAnswer.toString();
                        }
                    });
                } else if (group.type === "multiple_choice") {
                    const userAnswerOption = q.options.find(o => o.userAnswer);
                    if (userAnswerOption) {
                        studentAnswers[q.id] = userAnswerOption.text || "";
                    }
                } else if (group.type === "drag_drop") {
                    // Implement if you have user answers for drag & drop
                    // Usually, you store the array of matches here
                }
            });
        });
    }

    checkBtn.addEventListener("click", async () => {
        saveCurrentPageAnswers();

        const dtoList = [];
        for (const key in studentAnswers) {
            const value = studentAnswers[key];
            if (typeof value === 'string') {
                const optionId = parseInt(key.split("_")[1]) || parseInt(key);
                dtoList.push({
                    ExerciseOptionId: optionId,
                    UserAnswer: value
                });
            } else if (Array.isArray(value)) {
                value.forEach(v => {
                    dtoList.push({
                        ExerciseOptionId: parseInt(key),
                        UserAnswer: v.value
                    });
                });
            }
        }

        await submitAnswersToServer(dtoList);

        // Reload verified data
        const res = await fetch(dataUrl);
        currentExercise = await res.json();

        // Sync local answers from server data
        syncStudentAnswersFromServer();

        // Re-render current page and show evaluation
        answersChecked = true;
        renderCurrentPage();

        // Evaluate all answers based on synced answers
        const allCorrect = evaluateAll();

        if (allCorrect) {
            // Mark current lesson as completed in localStorage
            const lessonKey = `progress_sem${userSemester}_lesson${lessonVal}`;
            const completionKey = `${lessonKey}_completed`;

            if (!localStorage.getItem(completionKey)) {
                localStorage.setItem(completionKey, "true");

                // Calculate new progress percent
                let completedCount = 0;
                for (let option of lessonDropdown.options) {
                    const lId = option.dataset.lesson;
                    const key = `progress_sem${userSemester}_lesson${lId}_completed`;
                    if (localStorage.getItem(key) === "true") {
                        completedCount++;
                    }
                }

                const newProgressPercent = Math.floor((completedCount / totalLessons) * 100);

                // Save progress to server
                await fetch('/MyBook/SaveProgress', {
                    method: 'POST',
                    headers: { 'Content-Type': 'application/json' },
                    body: JSON.stringify({
                        bookId: bookIdFromRazorView,
                        progressPercent: newProgressPercent
                    })
                });

                // ✅ NEW: Notify parent to update
                window.parent.postMessage({ type: "exerciseCompleted" }, "*");

                // Update UI
                updateProgressBarFromValue(newProgressPercent);
            }

            checkBtn.disabled = true;
            checkBtn.textContent = "✔ تم التحقق من الإجابات بنجاح";
        }
        else {
            checkBtn.textContent = "✘ بعض الإجابات غير صحيحة، حاول مرة أخرى";
            checkBtn.classList.add("shake");
            setTimeout(() => checkBtn.classList.remove("shake"), 500);
        }

    });





    loadExercises();
});
