const buttons = document.querySelectorAll(".curved-btn");
const contents = document.querySelectorAll(".content");

buttons.forEach((btn, index) => {
  btn.addEventListener("click", () => {
    const isActive = btn.classList.contains("active");

    buttons.forEach((b, i) => {
      b.classList.remove("active", "above", "below");
      if (i < index) b.classList.add("above");
      if (i > index) b.classList.add("below");
    });

    if (!isActive) {
      btn.classList.add("active");
      contents.forEach((c, i) => {
        c.classList.toggle("active", i === index);
      });
    } else {
      buttons.forEach(b => b.classList.remove("above", "below"));
      contents.forEach(c => c.classList.remove("active"));
    }
  });
});



 const adBanner = document.getElementById('adBanner');
  const closeBtn = adBanner.querySelector('.close-btn');

  closeBtn.addEventListener('click', () => {
    adBanner.classList.add('hide');
  });