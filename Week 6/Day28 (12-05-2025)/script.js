const users = [
    { id: 1, name: "User1", email: "user1@example.com" },
    { id: 2, name: "User2", email: "user2@example.com" },
    { id: 3, name: "User3", email: "user3@example.com" }
];

const userList = document.getElementById('userList');
const errorDiv = document.getElementById('error');

function displayUsers(data) {
    userList.innerHTML = '';
    errorDiv.textContent = '';
    data.forEach(u => {
        const li = document.createElement('li');
        li.className = 'list-group-item';
        li.textContent = `${u.name} (${u.email})`;
        userList.appendChild(li);
    });
}

function displayError(message) {
    userList.innerHTML = '';
    errorDiv.textContent = message;
}

// Callback
function fetchData(callback) {
    setTimeout(() => {
        callback(null, users); 
    }, 1000);
}

function getUsersCallback() {
    fetchData((err, data) => {
        if (err) {
            displayError(err.message);
        } else {
            console.log(data);
            displayUsers(data);
        }
    });
}

// Promise
function getUsersPromise() {
    handlePromiseFetch()
        .then(data => {
            console.log(data);
            displayUsers(data)
        })
        .catch(err => displayError(err.message));
}

function handlePromiseFetch() {
    return new Promise((resolve) => {
        setTimeout(() => {
            resolve(users); 
        }, 1000);
    });
}

// Async/Await
async function getUsersAsync() {
    try {
        const data = await handlePromiseFetch(); 
        console.log(data);
        displayUsers(data);                   
    } catch (err) {
        displayError(err.message);
    }
}
