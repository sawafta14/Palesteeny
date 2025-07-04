document.addEventListener("DOMContentLoaded", function () {
    const form = document.getElementById("ai-form");
    const replyDiv = document.getElementById("ai-reply");
    const errorDiv = document.getElementById("ai-error");
    const chatBox = document.getElementById("ai-chat-box");
    const toggle = document.getElementById("ai-toggle-area");
    const container = document.getElementById("ai-assistant-container");

    const suggestionCloud = document.getElementById("cloud-suggestion");

    // 🌥️ Auto suggestion animation when not active
    let suggestionInterval = setInterval(() => {
        if (!container.classList.contains("active")) {
            container.classList.add("suggesting");

            setTimeout(() => {
                container.classList.remove("suggesting");
            }, 3000); // Slide out for 3 seconds
        }
    }, 10000); // Every 10 seconds

    // 🤖 Toggle assistant open/close
    toggle?.addEventListener("click", function () {
        const isOpen = chatBox.style.display === "block";
        chatBox.style.display = isOpen ? "none" : "block";

        container.classList.toggle("active", !isOpen);
        container.classList.remove("suggesting"); // stop cloud suggestion if opening
    });

    // 💬 Handle form submission
    if (form) {
        form.addEventListener("submit", function (e) {
            e.preventDefault();

            const formData = new FormData(form);
            const userMessage = formData.get("UserMessage");

            if (!userMessage || userMessage.trim() === "") {
                replyDiv.innerText = "يرجى إدخال رسالة.";
                return;
            }

            replyDiv.innerHTML = "<i>سؤال رائع! دعني افكر...</i>";
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

                    if (newReply) {
                        replyDiv.innerHTML = newReply.innerHTML;
                    } else {
                        replyDiv.innerHTML = "تعذر الحصول على الرد.";
                    }

                    form.reset();
                })
                .catch(error => {
                    replyDiv.innerText = "";
                    errorDiv.innerText = "حدث خطأ أثناء إرسال الرسالة.";
                    console.error(error);
                });
        });
    }
});

