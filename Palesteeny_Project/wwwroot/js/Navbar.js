document.addEventListener("DOMContentLoaded", function () {
    const profilePic = document.getElementById("profilePic");
    const dropdown = document.getElementById("profileDropdown");

    let isOpen = false;

    profilePic.addEventListener("click", function (event) {
        event.stopPropagation();
        isOpen = !isOpen;
        dropdown.classList.toggle("hidden", !isOpen);
    });

    document.addEventListener("click", function (event) {
        if (!dropdown.contains(event.target) && !profilePic.contains(event.target)) {
            dropdown.classList.add("hidden");
            isOpen = false;
        }
    });
});
