document.addEventListener("DOMContentLoaded", function () {
    const container = document.getElementById("exercise-container");
    const checkBtn = document.getElementById("check-answers");

    const params = new URLSearchParams(window.location.search);
    const classVal = params.get("class");
    const semVal = params.get("sem");
    const lessonVal = params.get("lesson");
    const dataUrl = `/exercises/class${classVal}/sem${semVal}/lesson${lessonVal}.json`;

    let currentExercise = null;
    let currentPageIndex = 0;
    const questionsPerPage = 2;
    const studentAnswers = {};

    async function loadExercises() {
        try {
            const res = await fetch(dataUrl);
            currentExercise = await res.json();

            if (!currentExercise || !currentExercise.pages || currentExercise.pages.length === 0) {
                container.innerHTML = "<p>لا توجد تمارين لهذا الدرس حتى الآن.</p>";
                return;
            }

            renderCurrentPage();
        } catch (err) {
            container.innerHTML = "<p>لا توجد تمارين لهذا الدرس حتى الآن.</p>";
            console.error("فشل تحميل التمرين:", err);
        }
    }

    function renderCurrentPage() {
        const start = currentPageIndex * questionsPerPage;
        const end = start + questionsPerPage;
        const pages = currentExercise.pages.slice(start, end);

        container.innerHTML = "";

        const pageWrapper = document.createElement("div");
        pageWrapper.className = "page-wrapper";

        pages.forEach((page, index) => {
            const arabicNumbers = ["السؤال الأول:", "السؤال الثاني:", "السؤال الثالث:", "السؤال الرابع:", ":السؤال الخامس", ":السؤال السادس", "السؤال السابع:", "السؤال الثامن:", "السؤال التاسع:", "السؤال العاشر:"];

            const block = document.createElement("div");
            block.className = `question-block ${index === 0 ? "block-top" : "block-bottom"}`;

            // ✅ NOW it's safe to add header to block
            const questionHeader = document.createElement("h2");
            questionHeader.className = "question-header";
            questionHeader.textContent = arabicNumbers[start + index] || `السؤال ${start + index + 1}`;
            block.appendChild(questionHeader);



            const inner = document.createElement("div");
            inner.className = "inner-block";

            const left = document.createElement("div");
            left.className = "left-side";

            const right = document.createElement("div");
            right.className = "right-side";

            // Question display
            let questionDisplay;
            if (page.questionOverlay) {
                questionDisplay = document.createElement("div");
                questionDisplay.className = "question-background";
                const text = document.createElement("div");
                text.className = "question-text";
                text.innerHTML = page.question;
                questionDisplay.appendChild(text);
            } else if (page.image) {
                questionDisplay = document.createElement("div");
                questionDisplay.className = "question-image-wrapper";
                questionDisplay.innerHTML = `<img src="${page.image}" alt="صورة"><div class="question-text-under">${page.question}</div>`;
            }

            // Answer block
            const answerBlock = document.createElement("div");
            answerBlock.className = "answer-block";

            if (page.type === "multiple_choice") {
                answerBlock.classList.add("choices");
                answerBlock.innerHTML = page.options.map(opt =>
                    `<button type="button" class="choice-btn" data-value="${opt}" data-name="${page.id}">${opt}</button>`
                ).join("");
            } else if (page.type === "select_option") {
                answerBlock.innerHTML = `<select name="${page.id}">
            <option value="">-- اختر --</option>
            ${page.options.map(opt => `<option value="${opt}">${opt}</option>`).join("")}
        </select>`;
            } else if (page.type === "text_input") {
                answerBlock.innerHTML = `<input type="text" name="${page.id}" placeholder="اكتب إجابتك هنا">`;
            } else if (page.type === "drag_drop") {
                const optionsHTML = page.items.map(item =>
                    `<div class="drag-item" draggable="true" data-val="${item.option}">${item.option}</div>`).join('');
                const targetsHTML = page.items.map(item => {
                    if (item.match.image) {
                        return `<div class="drop-zone" data-match="${item.match.label}">
                    <img src="${item.match.image}" alt="${item.match.label}" style="height:100px;"><br>${item.match.label}
                </div>`;
                    } else {
                        return `<div class="drop-zone" data-match="${item.match}">${item.match}</div>`;
                    }
                }).join('');
                answerBlock.innerHTML = `<div>${optionsHTML}</div><div style="margin-top:10px;">${targetsHTML}</div>`;
            }

            left.appendChild(questionDisplay);
            right.appendChild(answerBlock);



            inner.appendChild(left);
            inner.appendChild(right);
            block.appendChild(inner);
            pageWrapper.appendChild(block);
        });


        container.appendChild(pageWrapper);
        updateNavButtons();
        initDragDrop();
        restoreSelections(pages);
        bindAnswerEvents();
        const totalPages = Math.ceil(currentExercise.pages.length / questionsPerPage);
        checkBtn.style.display = (currentPageIndex === totalPages - 1) ? "block" : "none";

    }

    function restoreSelections(pages) {
        pages.forEach(page => {
            const answer = studentAnswers[page.id];
            if (!answer) return;

            if (page.type === "multiple_choice") {
                const btn = document.querySelector(`.choice-btn[data-name="${page.id}"][data-value="${answer}"]`);
                if (btn) btn.classList.add("selected");
            } else if (page.type === "select_option") {
                const sel = document.querySelector(`select[name="${page.id}"]`);
                if (sel) sel.value = answer;
            } else if (page.type === "text_input") {
                const inp = document.querySelector(`input[name="${page.id}"]`);
                if (inp) inp.value = answer;
            } else if (page.type === "drag_drop") {
                const zones = document.querySelectorAll(`.drop-zone`);
                answer.forEach(ans => {
                    zones.forEach(zone => {
                        if (zone.dataset.match === ans.target) {
                            zone.textContent = ans.value;
                            zone.dataset.current = ans.value;
                            zone.style.backgroundColor = "#d4ffd4";
                        }
                    });
                });
            }
        });
    }

    function bindAnswerEvents() {
        document.querySelectorAll(".choice-btn").forEach(btn => {
            btn.addEventListener("click", function () {
                const name = this.dataset.name;
                document.querySelectorAll(`.choice-btn[data-name="${name}"]`).forEach(b => b.classList.remove("selected"));
                this.classList.add("selected");
                studentAnswers[name] = this.dataset.value;
            });
        });
    }

    function saveCurrentAnswer() {
        const pages = currentExercise.pages.slice(currentPageIndex * questionsPerPage, (currentPageIndex + 1) * questionsPerPage);
        pages.forEach(page => {
            let answer;
            if (page.type === "multiple_choice") {
                const selected = document.querySelector(`.choice-btn[data-name="${page.id}"].selected`);
                answer = selected?.dataset.value;
            } else if (page.type === "select_option") {
                answer = document.querySelector(`select[name="${page.id}"]`)?.value;
            } else if (page.type === "text_input") {
                answer = document.querySelector(`input[name="${page.id}"]`)?.value.trim();
            } else if (page.type === "drag_drop") {
                const zones = document.querySelectorAll(`.drop-zone`);
                answer = Array.from(zones).map(z => ({
                    target: z.dataset.match,
                    value: z.dataset.current || ""
                }));
            }
            studentAnswers[page.id] = answer;
        });
    }

    function initDragDrop() {
        const drags = document.querySelectorAll(".drag-item");
        const drops = document.querySelectorAll(".drop-zone");

        // Enable dragging options
        drags.forEach(el => {
            el.setAttribute("draggable", true);
            el.addEventListener("dragstart", e => {
                e.dataTransfer.setData("text/plain", el.dataset.val);
                e.dataTransfer.effectAllowed = "move";
            });
        });

        // Make drop zones accept drags
        drops.forEach(zone => {
            zone.addEventListener("dragover", e => e.preventDefault());

            zone.addEventListener("drop", e => {
                e.preventDefault();
                const val = e.dataTransfer.getData("text/plain");

                // Check if already dropped
                const existing = zone.querySelector(".dropped-val");
                if (existing) return;

                // Create dropped value container
                const dropped = document.createElement("div");
                dropped.className = "dropped-val";
                dropped.textContent = val;
                dropped.setAttribute("draggable", true);
                dropped.style.marginTop = "8px";
                dropped.style.backgroundColor = "#d4ffd4";
                dropped.style.borderRadius = "8px";
                dropped.style.padding = "5px";
                dropped.style.cursor = "grab";

                // Allow dragging dropped value back out
                dropped.addEventListener("dragstart", ev => {
                    ev.dataTransfer.setData("text/plain", val);
                    ev.dataTransfer.effectAllowed = "move";
                    // Optional: remove from zone temporarily
                    setTimeout(() => dropped.remove(), 0);
                    delete zone.dataset.current;
                });

                // Save answer
                zone.appendChild(dropped);
                zone.dataset.current = val;
            });
        });

        // Allow dropping back into drag area
        const dragArea = document.querySelectorAll(".drag-item")[0]?.parentElement;
        if (dragArea) {
            dragArea.addEventListener("dragover", e => e.preventDefault());
            dragArea.addEventListener("drop", e => e.preventDefault()); // optional
        }
    }



    function updateNavButtons() {
        const totalPages = Math.ceil(currentExercise.pages.length / questionsPerPage);
        document.getElementById("prev-btn").disabled = currentPageIndex === 0;
        document.getElementById("next-btn").disabled = currentPageIndex >= totalPages - 1;
    }

    document.getElementById("prev-btn").addEventListener("click", () => {
        saveCurrentAnswer();
        if (currentPageIndex > 0) {
            currentPageIndex--;
            renderCurrentPage();
        }
    });

    document.getElementById("next-btn").addEventListener("click", () => {
        saveCurrentAnswer();
        currentPageIndex++;
        renderCurrentPage();
    });

    checkBtn.addEventListener("click", () => {
        saveCurrentAnswer();
        console.log("Student Answers:", studentAnswers);
        alert("تم جمع جميع الإجابات!");

        // ✅ Send a message to parent window (Ketabi)
        const lessonParam = new URLSearchParams(window.location.search).get("lesson");
        window.parent.postMessage({
            type: "exerciseCompleted",
            lesson: lessonParam
        }, "*"); // You can replace "*" with your origin for more security
    });


    loadExercises();
});