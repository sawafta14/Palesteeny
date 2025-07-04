let classDropdown, lessonDropdown, semDropdown;

document.addEventListener("DOMContentLoaded", function () {
    console.log("DOM fully loaded");
    let currentTab = "book";

    // -------------------- 1. DOM Elements --------------------
    classDropdown = document.getElementById('classDropdown');
    lessonDropdown = document.getElementById('lessonDropdown');
    semDropdown = document.getElementById('semDropdown');

    const setBookmarkBtn = document.getElementById("set-bookmark-btn");
    const goToBookmarkBtn = document.getElementById("go-to-bookmark-btn");
    const bookmarkModal = document.getElementById("bookmark-modal");
    const saveBookmarkBtn = document.getElementById("save-bookmark-btn");
    const cancelBookmarkBtn = document.getElementById("cancel-bookmark-btn");
    const bookmarkLessonSelect = document.getElementById("bookmarkLessonSelect");

    const tabs = document.querySelectorAll(".tab");
    const displayArea = document.querySelector(".display-area");
    const bookmarkButtonsContainer = document.querySelector(".bookmark-buttons");

    // -------------------- 2. Bookmark Handlers --------------------
    setBookmarkBtn.addEventListener("click", () => {
        bookmarkLessonSelect.value = lessonDropdown.value;
        bookmarkModal.style.display = "flex";
    });

    cancelBookmarkBtn.addEventListener("click", () => {
        bookmarkModal.style.display = "none";
    });

    saveBookmarkBtn.addEventListener("click", () => {
        const bookmark = {
            classValue: classDropdown.value,
            semValue: semDropdown.value,
            lessonValue: bookmarkLessonSelect.value
        };
        localStorage.setItem("ketabiBookmark", JSON.stringify(bookmark));
        bookmarkModal.style.display = "none";

        // Explicitly activate the Book tab
        tabs.forEach(t => t.classList.remove("active"));
        const bookTab = document.querySelector('.tab[data-tab="book"]');
        bookTab.classList.add("active");
        currentTab = "book";
        bookmarkButtonsContainer.style.display = "flex";
        switchToBook();
    });

    goToBookmarkBtn.addEventListener("click", () => {
        switchToBookmark();

        // Force Book tab to activate visually and logically
        setTimeout(() => {
            tabs.forEach(t => t.classList.remove("active"));
            const bookTab = document.querySelector('.tab[data-tab="book"]');
            bookTab.classList.add("active");
            currentTab = "book";
            bookmarkButtonsContainer.style.display = "flex";
        }, 10);
    });

    function saveBookmark() {
        const bookmark = {
            classValue: classDropdown.value,
            semValue: semDropdown.value,
            lessonValue: lessonDropdown.value
        };
        localStorage.setItem("ketabiBookmark", JSON.stringify(bookmark));
    }

    function switchToBookmark() {
        const bookmarkData = JSON.parse(localStorage.getItem("ketabiBookmark"));
        if (!bookmarkData) {
            alert("لم يتم تحديد علامة من قبل.");
            return;
        }

        // Temporarily remove listeners to prevent triggering changes
        [classDropdown, semDropdown, lessonDropdown].forEach(dropdown => {
            dropdown.removeEventListener('change', dropdownChangeHandler);
        });

        classDropdown.value = bookmarkData.classValue;
        semDropdown.value = bookmarkData.semValue;
        lessonDropdown.value = bookmarkData.lessonValue;

        const pdfSrc = getPdfPath(bookmarkData.classValue, bookmarkData.semValue, bookmarkData.lessonValue);
        const newIframe = document.createElement('iframe');
        newIframe.type = "application/pdf";
        newIframe.src = pdfSrc;
        newIframe.setAttribute("allowfullscreen", false);

        displayArea.classList.remove("video-mode");
        displayArea.innerHTML = '';
        displayArea.appendChild(newIframe);
        bookmarkButtonsContainer.style.display = "flex";

        // Reattach listeners
        [classDropdown, semDropdown, lessonDropdown].forEach(dropdown => {
            dropdown.addEventListener('change', dropdownChangeHandler);
        });
    }

    // -------------------- 3. Tab Switching Functions --------------------
    function switchToBook() {
        displayArea.classList.remove("video-mode");
        updateIframeSrc();
    }

    function switchToVideo() {
        const classValue = classDropdown.value;
        const lessonValue = lessonDropdown.value;
        const semValue = semDropdown.value;

        const videoSrc = (
            videoLinks[classValue]?.[semValue]?.[lessonValue]
        ) || "https://www.youtube.com/embed/default";

        const newIframe = document.createElement("iframe");
        newIframe.src = videoSrc;
        newIframe.setAttribute("allowfullscreen", true);

        displayArea.innerHTML = "";
        displayArea.appendChild(newIframe);
        displayArea.classList.add("video-mode");
    }

    function switchToExercise() {
        displayArea.classList.remove("video-mode");

        const classValue = classDropdown.value;
        const semValue = semDropdown.value;
        const lessonPage = lessonDropdown.value;
        const lessonNumber = lessonDropdown.selectedOptions[0]?.dataset.lesson;
        const exercisePage = `/exercises/exercises.html?class=${classValue}&sem=${semValue}&lesson=${lessonNumber}&page=${lessonPage}`;

        const iframe = document.createElement("iframe");
        iframe.src = exercisePage;
        iframe.style.width = "100%";
        iframe.style.height = "100%";
        iframe.setAttribute("allowfullscreen", true);

        displayArea.innerHTML = "";
        displayArea.appendChild(iframe);
    }

    function updateIframeSrc() {
        const classValue = classDropdown.value;
        const lessonValue = lessonDropdown.value;
        const semValue = semDropdown.value;

        const newSrc = getPdfPath(classValue, semValue, lessonValue);

        displayArea.classList.remove("video-mode");

        const newIframe = document.createElement('iframe');
        newIframe.type = "application/pdf";
        newIframe.src = newSrc;
        newIframe.setAttribute("allowfullscreen", false);

        displayArea.innerHTML = '';
        displayArea.appendChild(newIframe);
    }

    function getPdfPath(classValue, semValue, pageNumber) {
        return `/books/class${classValue}book${semValue}.pdf#page=${pageNumber}`;
    }

    function getActiveTab() {
        return currentTab;
    }

    // -------------------- 4. Event Listeners --------------------
    function dropdownChangeHandler() {
        const tab = getActiveTab();
        if (tab === "book") updateIframeSrc();
        else if (tab === "video") switchToVideo();
        else if (tab === "quiz") switchToExercise();
        saveBookmark();
    }

    [classDropdown, semDropdown, lessonDropdown].forEach(dropdown => {
        dropdown.addEventListener('change', dropdownChangeHandler);
    });

    tabs.forEach(tab => {
        tab.addEventListener("click", function () {
            tabs.forEach(t => t.classList.remove("active"));
            tab.classList.add("active");
            const tabName = tab.getAttribute("data-tab");
            currentTab = tabName;

            if (tabName === "video") {
                switchToVideo();
                bookmarkButtonsContainer.style.display = "none";
            } else if (tabName === "quiz") {
                switchToExercise();
                bookmarkButtonsContainer.style.display = "none";
            } else if (tabName === "book") {
                switchToBook();
                bookmarkButtonsContainer.style.display = "flex";
            }
        });
    });

    // -------------------- 5. Initial Setup --------------------
    const bookTab = document.querySelector('.tab[data-tab="book"]');
    bookTab.classList.add("active");
    currentTab = "book";
    bookmarkButtonsContainer.style.display = "flex";
    updateIframeSrc();
    setTimeout(updateProgressBar, 100);

    // -------------------- 6. Video Links --------------------
    const videoLinks = {
        1: {
            1: {
                6: "https://www.youtube.com/embed/5sA7lWoIKkg",
                24: "https://www.youtube.com/embed/bLYM1hAzV1w",
                35: "https://www.youtube.com/embed/tWTW2RiCzC4"
            },
            2: {
                6: "https://www.youtube.com/embed/VIDEO_ID7",
                24: "https://www.youtube.com/embed/VIDEO_ID8",
                35: "https://www.youtube.com/embed/VIDEO_ID9"
            }
        }

    };
    // -------------------- 7. Progress bar --------------------

    function getProgressKey() {
        const cls = classDropdown?.value || "0";
        const sem = semDropdown?.value || "0";
        const lesson = lessonDropdown?.value || "0";
        return `progress_class${cls}_sem${sem}_lesson${lesson}`;
    }


    function getLessonProgress() {
        const progressKey = getProgressKey();
        return parseInt(localStorage.getItem(progressKey) || "0", 10);
    }

    function setLessonProgress(percent) {
        const progressKey = getProgressKey();
        localStorage.setItem(progressKey, percent);
        updateProgressBar();
    }

    function updateProgressBar() {
        const progress = getLessonProgress();
        const bar = document.getElementById("lesson-progress-bar");
        bar.style.width = `${progress}%`;
        bar.textContent = `${progress}%`;
    }

    // Call this every time dropdowns change
    [classDropdown, semDropdown, lessonDropdown].forEach(dropdown => {
        dropdown.addEventListener('change', updateProgressBar);
    });
    window.addEventListener("message", function (event) {
        if (event.data?.type === "exerciseCompleted") {
            const lessonKey = `progress_class${classDropdown.value}_sem${semDropdown.value}_lesson${lessonDropdown.value}`;
            const completionKey = `${lessonKey}_completed`;

            if (localStorage.getItem(completionKey)) return; // already marked done

            localStorage.setItem(completionKey, "true");

            const currentProgress = getLessonProgress();
            const newProgress = Math.min(currentProgress + 10, 100);
            setLessonProgress(newProgress);
            updateProgressBar();

        }
    });


});
