document.addEventListener("DOMContentLoaded", function () {
    // ========== Elements ==========
    const form = document.getElementById("ai-form");
    const replyDiv = document.getElementById("ai-reply");
    const errorDiv = document.getElementById("ai-error");
    const chatBox = document.getElementById("ai-chat-box");
    const toggle = document.getElementById("ai-toggle-area");
    const container = document.getElementById("ai-assistant-container");
    const suggestionCloud = document.getElementById("cloud-suggestion");
    const currentPageInput = document.getElementById("currentPageInput");
    const assistantIcon = document.querySelector(".teeny-icon");

    // ========== Set Current Page Path ==========
    if (currentPageInput) {
        currentPageInput.value = window.location.pathname.toLowerCase();
        console.log("📄 Current page path sent to AI:", currentPageInput.value);
    }

    // ========== Assistant Toggle & Suggestion Animation ==========
    if (container && toggle && chatBox) {
        setInterval(() => {
            if (!container.classList.contains("active")) {
                container.classList.add("suggesting");
                setTimeout(() => {
                    container.classList.remove("suggesting");
                }, 3000);
            }
        }, 10000);

        toggle.addEventListener("click", function () {
            const isOpen = chatBox.style.display === "block";
            chatBox.style.display = isOpen ? "none" : "block";
            container.classList.toggle("active", !isOpen);
            container.classList.remove("suggesting");
        });
    }

    // ========== Submit Chat ==========
    if (form && replyDiv && errorDiv) {
        form.addEventListener("submit", function (e) {
            e.preventDefault();

            const formData = new FormData(form);
            const userMessage = formData.get("UserMessage");

            if (!userMessage || userMessage.trim() === "") {
                replyDiv.innerText = "يرجى إدخال رسالة.";
                return;
            }

            replyDiv.innerHTML = "<i>سؤال رائع! دعني أفكر...</i>";
            errorDiv.innerHTML = "";

            fetch("/Chat/AiAssistant", {
                method: "POST",
                body: formData
            })
                .then(response => response.text())
                .then(html => {
                    const parser = new DOMParser();
                    const doc = parser.parseFromString(html, "text/html");
                    const newReply = doc.querySelector("#ai-reply");

                    replyDiv.innerHTML = newReply ? newReply.innerHTML : "تعذر الحصول على الرد.";
                    form.reset();
                })
                .catch(error => {
                    replyDiv.innerText = "";
                    errorDiv.innerText = "حدث خطأ أثناء إرسال الرسالة.";
                    console.error(error);
                });
        });
    }

    // ========== Load Assistant Icon ==========
    // Load assistant icon dynamically
    if (assistantIcon) {
        fetch("/Chat/LoadAssistant")
            .then(res => res.json())
            .then(data => {
                if (data.aiImage) {
                    assistantIcon.src = data.aiImage;
                    console.log("✅ Loaded assistant icon:", data.aiImage);
                }
            })
            .catch(error => {
                console.error("❌ Failed to load assistant image:", error);
            });
    }

    document.addEventListener("DOMContentLoaded", function () {
        const assistantImg = document.querySelector('#ai-toggle-area img');
        if (!assistantImg) return;

        fetch("/Chat/LoadAssistant")
            .then(res => res.json())
            .then(data => {
                if (data.aiImage) {
                    assistantImg.src = data.aiImage;
                    console.log("✅ تم تحميل صورة المساعد:", data.aiImage);
                }
            })
            .catch(err => {
                console.error("❌ خطأ في تحميل صورة المساعد:", err);
            });
    });



});
