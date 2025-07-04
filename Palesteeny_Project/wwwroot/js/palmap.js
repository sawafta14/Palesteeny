const zoomInBtn = document.getElementById("zoom-in");
const zoomOutBtn = document.getElementById("zoom-out");
const palmap = document.getElementById("palmap");

let scale = 1.5;
let offsetX = 0;
let offsetY = 0;
let isDragging = false;
let startX = 0;
let startY = 0;

zoomInBtn.addEventListener("click", () => {
    scale += 0.2;
    updateImageTransform();
});

zoomOutBtn.addEventListener("click", () => {
    if (scale > 0.5) {
        scale -= 0.2;
        updateImageTransform();
    }
});

function updateImageTransform() {
    palmap.style.transform = `scale(${scale}) translate(${offsetX}px, ${offsetY}px)`;
}

palmap.addEventListener("mousedown", (e) => {
    isDragging = true;
    startX = e.clientX - offsetX;
    startY = e.clientY - offsetY;
    palmap.style.cursor = "grabbing";
});

palmap.addEventListener("mousemove", (e) => {
    if (isDragging) {
        offsetX = e.clientX - startX;
        offsetY = e.clientY - startY;
        updateImageTransform();
    }
});

palmap.addEventListener("mouseup", () => {
    isDragging = false;
    palmap.style.cursor = "grab";
});

palmap.addEventListener("mouseleave", () => {
    isDragging = false;
    palmap.style.cursor = "grab";
});
const fromPoint = document.getElementById('fromPoint');
const toPoint = document.getElementById('toPoint');
const routeLine = document.getElementById('routeLine');
const searchHint = document.getElementById('searchHint');

// ?????? ???? ?? ????? ?? ???path
function getCityCenter(cityId) {
    const city = document.getElementById(cityId);
    if (!city) return null;
    const bbox = city.getBBox();
    return {
        x: bbox.x + bbox.width / 2,
        y: bbox.y + bbox.height / 2
    };
}

// ??? ????? ??? "????? ??????"
document.querySelector('.show-route').addEventListener('click', () => {
    const fromCity = document.getElementById('from').value;
    const toCity = document.getElementById('to').value;
    if (fromCity === "???? ?????" || toCity === "???? ?????") return;

    const from = getCityCenter(fromCity);
    const to = getCityCenter(toCity);
    if (!from || !to) return;

    fromPoint.setAttribute('cx', from.x);
    fromPoint.setAttribute('cy', from.y);
    fromPoint.setAttribute('visibility', 'visible');

    toPoint.setAttribute('cx', to.x);
    toPoint.setAttribute('cy', to.y);
    toPoint.setAttribute('visibility', 'visible');

    routeLine.setAttribute('x1', from.x);
    routeLine.setAttribute('y1', from.y);
    routeLine.setAttribute('x2', to.x);
    routeLine.setAttribute('y2', to.y);
    routeLine.setAttribute('visibility', 'visible');
    routeLine.setAttribute('stroke', 'black');           // ??? ????

    routeLine.setAttribute('stroke-width', '8');        // ???????
    routeLine.setAttribute('stroke-dasharray', '15,5'); // ???? ???? ?????

    routeLine.setAttribute('stroke-dashoffset', 1000);

    setTimeout(() => {
        routeLine.setAttribute('stroke-dashoffset', 0);
    }, 100);
});

// ??? ??????? ?? ?????
document.querySelector('.city-search').addEventListener('input', e => {
    const value = e.target.value.trim();
    const city = document.getElementById(value);
    if (city) {
        const center = getCityCenter(value);
        searchHint.setAttribute('x', center.x + 10);
        searchHint.setAttribute('y', center.y);
        searchHint.setAttribute('visibility', 'visible');
    } else {
        searchHint.setAttribute('visibility', 'hidden');
    }
});
document.querySelector('.cancel').addEventListener('click', () => {
    fromPoint.setAttribute('visibility', 'hidden');
    toPoint.setAttribute('visibility', 'hidden');
    routeLine.setAttribute('visibility', 'hidden');
    routeLine.setAttribute('stroke-dashoffset', 1000);
});