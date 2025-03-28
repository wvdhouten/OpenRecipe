export function createToast(message) {
    const toastContainer = document.getElementById('toast-container');
    const template = document.getElementById('toast-template');
    const clone = template.content.cloneNode(true).lastElementChild;
    clone.querySelector('.toast-body').textContent = message;

    toastContainer.appendChild(clone);
    bootstrap.Toast.getOrCreateInstance(clone).show();
}