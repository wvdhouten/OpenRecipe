let DATABASE_NAME = "Open Recipe";

export function initialize(dbVersion, collectionNames) {
    let request = indexedDB.open(DATABASE_NAME, dbVersion);
    request.onupgradeneeded = function () {
        let db = request.result;
        collectionNames.forEach(collectionName => {
            if (!db.objectStoreNames.contains(collectionName))
                db.createObjectStore(collectionName, { keyPath: "id" });
        });
    }
}

export async function getAll(collectionName) {
    let request = new Promise((resolve) => {
        let db = indexedDB.open(DATABASE_NAME);
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
        let db = indexedDB.open(DATABASE_NAME);
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
    let request = indexedDB.open(DATABASE_NAME);
    request.onsuccess = function () {
        let transaction = request.result.transaction(collectionName, "readwrite");
        let collection = transaction.objectStore(collectionName)
        collection.put(value);
    }
}

export function remove(collectionName, value) {
    let request = indexedDB.open(DATABASE_NAME);
    request.onsuccess = function () {
        let transaction = request.result.transaction(collectionName, "readwrite");
        let collection = transaction.objectStore(collectionName)
        collection.delete(value);
    }
}

export function clear(collectionName) {
    let request = indexedDB.open(DATABASE_NAME);
    request.onsuccess = function () {
        let transaction = request.result.transaction(collectionName, "readwrite");
        let collection = transaction.objectStore(collectionName)
        collection.clear();
    }
}
