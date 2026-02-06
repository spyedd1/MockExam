const body = document.body;

// --------------------
// Dark Mode
// --------------------
function toggleDarkMode() {
    body.classList.toggle("dark-mode");
    localStorage.setItem(
        "theme",
        body.classList.contains("dark-mode") ? "dark" : "light"
    );
}

if (localStorage.getItem("theme") === "dark") {
    body.classList.add("dark-mode");
}

window.toggleDarkMode = toggleDarkMode;

// --------------------
// High Contrast Mode
// --------------------
function toggleHighContrast() {
    body.classList.toggle("high-contrast");
    localStorage.setItem(
        "contrastMode",
        body.classList.contains("high-contrast") ? "high-contrast" : "normal"
    );
}

if (localStorage.getItem("contrastMode") === "high-contrast") {
    body.classList.add("high-contrast");
}

window.toggleHighContrast = toggleHighContrast;

// --------------------
// Text to Speech
// --------------------
function speakText() {
    const text = document.body.innerText;
    const speech = new SpeechSynthesisUtterance(text);
    speech.lang = "en-GB";
    window.speechSynthesis.cancel(); // stops overlap
    window.speechSynthesis.speak(speech);
}

window.speakText = speakText;

// --------------------
// Font Size
// --------------------
function changeFontSize(action) {
    let html = document.documentElement;
    let currentSize = parseFloat(
        window.getComputedStyle(html).fontSize
    );

    if (action === "increase") currentSize += 2;
    if (action === "decrease") currentSize -= 2;

    currentSize = Math.max(12, Math.min(22, currentSize));

    html.style.fontSize = currentSize + "px";
    localStorage.setItem("fontSize", currentSize);
}

// Restore on load
const savedFontSize = localStorage.getItem("fontSize");
if (savedFontSize) {
    document.documentElement.style.fontSize = savedFontSize + "px";
}


window.changeFontSize = changeFontSize;
