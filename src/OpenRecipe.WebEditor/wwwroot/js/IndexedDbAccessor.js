let DATABASE_NAME = "Open Recipe";
let CURRENT_VERSION = 1;

export function initialize(collectionName) {
    let request = indexedDB.open(DATABASE_NAME, CURRENT_VERSION);
    request.onupgradeneeded = function () {
        let db = request.result;
        db.createObjectStore(collectionName, { keyPath: "id" });
    }
}

export async function getAll(collectionName) {
    let request = new Promise((resolve) => {
        let db = indexedDB.open(DATABASE_NAME, CURRENT_VERSION);
        db.onsuccess = function () {
            let transaction = db.result.transaction(collectionName, "readonly");
            let collection = transaction.objectStore(collectionName);
            let result = collection.getAll();

            result.onsuccess = function (e) {
                resolve(result.result);
            }
        }
    });

    return await request;
}

export async function get(collectionName, id) {
    let request = new Promise((resolve) => {
        let db = indexedDB.open(DATABASE_NAME, CURRENT_VERSION);
        db.onsuccess = function () {
            let transaction = db.result.transaction(collectionName, "readonly");
            let collection = transaction.objectStore(collectionName);
            let result = collection.get(id);

            result.onsuccess = function (e) {
                resolve(result.result);
            }
        }
    });

    return await request;
}

export function set(collectionName, value) {
    let request = indexedDB.open(DATABASE_NAME, CURRENT_VERSION);
    request.onsuccess = function () {
        let transaction = request.result.transaction(collectionName, "readwrite");
        let collection = transaction.objectStore(collectionName)
        collection.put(value);
    }
}

export function remove(collectionName, value) {
    let request = indexedDB.open(DATABASE_NAME, CURRENT_VERSION);
    request.onsuccess = function () {
        let transaction = request.result.transaction(collectionName, "readwrite");
        let collection = transaction.objectStore(collectionName)
        collection.delete(value);
    }
}

export function clear(collectionName) {
    let request = indexedDB.open(DATABASE_NAME, CURRENT_VERSION);
    request.onsuccess = function () {
        let transaction = request.result.transaction(collectionName, "readwrite");
        let collection = transaction.objectStore(collectionName)
        collection.clear();
    }
}
