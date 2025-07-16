document.addEventListener("DOMContentLoaded", function () {
    console.log("DOM fully loaded");
    let currentTab = "book";

    // userSemester must be set globally in Razor view before this script loads
    if (typeof userSemester === 'undefined') {
        console.error("userSemester is not defined!");
        return;
    }

    // -------------------- 1. DOM Elements --------------------
    const lessonDropdown = document.getElementById('lessonDropdown');
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
        const selectedLessonId = bookmarkLessonSelect.selectedOptions[0].dataset.lesson;

        fetch('/MyBook/SaveBookmark', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({ lessonId: parseInt(selectedLessonId) })
        })
            .then(response => {
                if (response.ok) {
                    bookmarkModal.style.display = "none";
                    switchToBook();
                } else {
                    alert("فشل حفظ العلامة.");
                }
            });
    });


    goToBookmarkBtn.addEventListener("click", () => {
        switchToBookmark();

        setTimeout(() => {
            tabs.forEach(t => t.classList.remove("active"));
            const bookTab = document.querySelector('.tab[data-tab="book"]');
            bookTab.classList.add("active");
            currentTab = "book";
            bookmarkButtonsContainer.style.display = "flex";
        }, 10);
    });

    function saveBookmark() {
        const bookmark = { lessonValue: lessonDropdown.value };
        localStorage.setItem("ketabiBookmark", JSON.stringify(bookmark));
    }

    function switchToBookmark() {
        fetch('/MyBook/GetBookmarkPage')
            .then(response => response.json())
            .then(data => {
                if (!data.success) {
                    alert("لا توجد علامة محفوظة.");
                    return;
                }

                const pdfSrc = getPdfPath(data.page);
                const newIframe = document.createElement('iframe');
                newIframe.type = "application/pdf";
                newIframe.src = pdfSrc;
                newIframe.setAttribute("allowfullscreen", false);

                displayArea.classList.remove("video-mode");
                displayArea.innerHTML = '';
                displayArea.appendChild(newIframe);
                bookmarkButtonsContainer.style.display = "flex";
            });
    }


    // -------------------- 3. Tab Switching Functions --------------------
    function switchToBook() {
        displayArea.classList.remove("video-mode");
        updateIframeSrc();
    }

    function cleanUrl(url) {
        return url.replace(/[\r\n]+/g, '').trim();
    }

    function switchToVideo() {
        const lessonValue = String(lessonDropdown.value);
        const rawUrl = videoLinks[userSemester]?.[lessonValue] || "";
        const videoSrc = cleanUrl(rawUrl);

        console.log("Lesson value:", lessonValue);
        console.log("userSemester:", userSemester);
        console.log("videoLinks:", videoLinks);
        console.log("Cleaned Video URL:", videoSrc);

        const newIframe = document.createElement("iframe");
        newIframe.src = videoSrc || "https://www.youtube.com/embed/default";
        newIframe.setAttribute("allowfullscreen", true);
        newIframe.style.width = "100%";
        newIframe.style.height = "100%";

        displayArea.innerHTML = "";
        displayArea.appendChild(newIframe);
        displayArea.classList.add("video-mode");
    }




    function switchToExercise() {
        displayArea.classList.remove("video-mode");

        const lessonValue = lessonDropdown.value;
        const lessonNumber = lessonDropdown.selectedOptions[0]?.dataset.lesson;
        const exercisePage = `/exercises/exercises.html?sem=${userSemester}&lesson=${lessonNumber}&page=${lessonValue}`;

        const iframe = document.createElement("iframe");
        iframe.src = exercisePage;
        iframe.style.width = "100%";
        iframe.style.height = "100%";
        iframe.setAttribute("allowfullscreen", true);

        displayArea.innerHTML = "";
        displayArea.appendChild(iframe);
    }

    function updateIframeSrc() {
        const lessonValue = lessonDropdown.value;
        const newSrc = getPdfPath(lessonValue);

        displayArea.classList.remove("video-mode");

        const newIframe = document.createElement('iframe');
        newIframe.type = "application/pdf";
        newIframe.src = newSrc;
        newIframe.setAttribute("allowfullscreen", false);

        displayArea.innerHTML = '';
        displayArea.appendChild(newIframe);
    }

    function getPdfPath(pageNumber) {
        return `/books/book${userSemester}.pdf#page=${pageNumber}`;
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

    lessonDropdown.addEventListener('change', dropdownChangeHandler);

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

    // -------------------- 6. Progress bar --------------------
    function getProgressKey() {
        const lesson = lessonDropdown?.value || "0";
        return `progress_sem${userSemester}_lesson${lesson}`;
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

    lessonDropdown.addEventListener('change', updateProgressBar);

    window.addEventListener("message", function (event) {
        if (event.data?.type === "exerciseCompleted") {
            const lessonKey = `progress_sem${userSemester}_lesson${lessonDropdown.value}`;
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
