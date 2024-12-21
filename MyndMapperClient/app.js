// Endpoints
const ORIGIN = "http://localhost:5110/";
const USER_ENDPOINT = "users/";
const CANVAS_ENDPOINT = "canvases/";
const GET = "get/";
const GET_ALL = "get/all";
const POST = "create/";
const PUT = "edit/";
const DELETE = "delete/";
const DELETE_ALL = "delete/all";

//#region DTOs
class UserGetDto {
    id
    name
    email
    password
    createdCanvases
}
class UserPostDto {
    name
    email
    password
}
class UserPutDto {
    id
    name
    email
    password
}
class CanvasGetDto {
    id
    name
    creationDate
    ownerId
}
class CanvasPostDto {
    ownerId
    name
}
class CanvasPutDto {
    id
    name
}
//#endregion

function as(type, object) {
    return Object.assign(new type, object);
}

function extractDict(event) {
    event.preventDefault();
    formData = new FormData(event.target);
    return Object.fromEntries(formData);
}

function clearNode(target) {
    target.innerHTML = "";
}

function addText(target, text) {
    let p = document.createElement('p');
    p.innerHTML = text;
    target.appendChild(p);
}

function addList(target, elements) {
    let ul = document.createElement('ul')
    for (i in elements) {
        let li = document.createElement('li');
        li.innerHTML = elements[i];
        ul.appendChild(li);
    }
    target.appendChild(ul);
}

function addClearButton(target) {
    let btn = document.createElement('button');
    btn.addEventListener('click', function () { clearNode(target) });
    btn.innerHTML = "clear";
    target.appendChild(btn);
}

//#region UserMethods
async function getUser(formData) {
    clearNode(userGetResponse);
    let targetId = formData["id"];
    let response = await fetch(ORIGIN + USER_ENDPOINT + GET + targetId);

    if (!response.ok) {
        addText(userGetResponse, "User not found.");
        addClearButton(userGetResponse);
        return;
    }

    let data = await response.json();
    let user = as(UserGetDto, data);
    addText(userGetResponse, "User info: ");
    info = [];
    info.push("id: " + user.id);
    info.push("name: " + user.name);
    info.push("email: " + user.email);
    info.push("password: " + user.password);
    info.push("created canvases: " + user.createdCanvases);
    addList(userGetResponse, info);
    addClearButton(userGetResponse);
}

async function getAllUsers() {
    clearNode(userGetAllResponse);
    let response = await fetch(ORIGIN + USER_ENDPOINT + GET_ALL);

    if (!response.ok) {
        addText(userGetAllResponse, "Something went wrong.");
        addClearButton(userGetAllResponse);
        return;
    }

    let data = await response.json();
    let convertedData = [];
    for (i in data) {
        convertedData.push(as(UserGetDto, data[i]));
    }
    for (i in convertedData) {
        addText(userGetAllResponse, "User: ");
        let user = convertedData[i];
        info = [];
        info.push("id: " + user.id);
        info.push("name: " + user.name);
        info.push("email: " + user.email);
        info.push("password: " + user.password);
        info.push("created canvases: " + user.createdCanvases);
        addList(userGetAllResponse, info);
    }
    addClearButton(userGetAllResponse);
}

async function postUser(formData) {
    clearNode(userPostResponse);
    let body = new UserPostDto;
    body.name = formData["name"];
    body.email = formData["email"];
    body.password = formData["password"];

    let response = await fetch(ORIGIN + USER_ENDPOINT + POST, {
        method: "post",
        headers: {
            'Accept': '*/*',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(body),
    });

    if (!response.ok) {
        let data = await response.text();
        addText(userPostResponse, data);
        addClearButton(userPostResponse);
        return;
    }

    addText(userPostResponse, "User created successfully.");
    addClearButton(userPostResponse);
}

async function putUser(formData) {
    clearNode(userPutResponse);
    let body = new UserPostDto;
    body.id = formData["id"];
    body.name = formData["name"];
    body.email = formData["email"];
    body.password = formData["password"];

    let response = await fetch(ORIGIN + USER_ENDPOINT + PUT, {
        method: "put",
        headers: {
            'Accept': '*/*',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(body),
    });

    if (!response.ok) {
        let data = await response.text();
        addText(userPutResponse, data);
        addClearButton(userPutResponse);
        return;
    }

    addText(userPutResponse, "User edited successfully.");
    addClearButton(userPutResponse);
}

async function deleteUser(formData) {
    clearNode(userDeleteResponse);
    let response = await fetch(ORIGIN + USER_ENDPOINT + DELETE + formData["id"], {
        method: "delete"
    });

    if (!response.ok) {
        addText(userDeleteResponse, "User not found.");
        addClearButton(userDeleteResponse);
        return;
    }

    addText(userDeleteResponse, "User deleted successfully.");
    addClearButton(userDeleteResponse);
}

async function deleteAllUsers() {
    clearNode(userDeleteAllResponse);
    let response = await fetch(ORIGIN + USER_ENDPOINT + DELETE_ALL, {
        method: "delete"
    });

    if (!response.ok) {
        addText(userDeleteAllResponse, "Operation denied.");
        addClearButton(userDeleteAllResponse);
        return;
    }

    addText(userDeleteAllResponse, "Users removed.");
    addClearButton(userDeleteAllResponse);
}
//#endregion

//#region CanvasMethods
async function getCanvas(formData) {
    clearNode(canvasGetResponse);
    let targetId = formData["id"];
    let response = await fetch(ORIGIN + CANVAS_ENDPOINT + GET + targetId);

    if (!response.ok) {
        addText(canvasGetResponse, "Canvas not found.");
        addClearButton(canvasGetResponse);
        return;
    }

    let data = await response.json();
    let canvas = as(CanvasGetDto, data);
    addText(canvasGetResponse, "Canvas info: ");
    info = [];
    info.push("id: " + canvas.id);
    info.push("name: " + canvas.name);
    info.push("creation date: " + canvas.creationDate);
    info.push("owner: " + canvas.ownerId);
    addList(canvasGetResponse, info);
    addClearButton(canvasGetResponse);
}

async function getAllCanvases() {
    clearNode(canvasGetAllResponse);
    let response = await fetch(ORIGIN + CANVAS_ENDPOINT + GET_ALL);

    if (!response.ok) {
        addText(canvasGetAllResponse, "Somnething went wrong.");
        addClearButton(userGetAllResponse);
        return;
    }

    let data = await response.json();
    let convertedData = [];
    for (i in data) {
        convertedData.push(as(CanvasGetDto, data[i]));
    }
    for (i in convertedData) {
        addText(canvasGetAllResponse, "Canvas: ");
        let user = convertedData[i];
        info = [];
        info.push("id: " + user.id);
        info.push("name: " + user.name);
        info.push("creation date: " + user.creationDate);
        info.push("owner: " + user.ownerId);
        addList(canvasGetAllResponse, info);
    }
    addClearButton(canvasGetAllResponse);
}

async function postCanvas(formData) {
    clearNode(canvasPostResponse);
    let body = new CanvasPostDto;
    body.name = formData["name"];
    body.ownerId = formData["id"];

    let response = await fetch(ORIGIN + CANVAS_ENDPOINT + POST, {
        method: "post",
        headers: {
            'Accept': '*/*',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(body),
    });

    if (!response.ok) {
        let data = await response.text();
        addText(canvasPostResponse, data);
        addClearButton(canvasPostResponse);
        return;
    }

    addText(canvasPostResponse, "Canvas created successfully.");
    addClearButton(canvasPostResponse);
}

async function putCanvas(formData) {
    clearNode(canvasPutResponse);
    let body = new CanvasPostDto;
    body.id = formData["id"];
    body.name = formData["name"];

    let response = await fetch(ORIGIN + CANVAS_ENDPOINT + PUT, {
        method: "put",
        headers: {
            'Accept': '*/*',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(body),
    });

    if (!response.ok) {
        let data = await response.text();
        addText(canvasPutResponse, data);
        addClearButton(canvasPutResponse);
        return;
    }

    addText(canvasPutResponse, "Canvas edited successfully.");
    addClearButton(userPutResponse);
}

async function deleteCanvas(formData) {
    clearNode(canvasDeleteResponse);
    let response = await fetch(ORIGIN + CANVAS_ENDPOINT + DELETE + formData["id"], {
        method: "delete"
    });

    if (!response.ok) {
        addText(canvasDeleteResponse, "Canvas not found.");
        addClearButton(canvasDeleteResponse);
        return;
    }

    addText(canvasDeleteResponse, "Canvas deleted successfully.");
    addClearButton(canvasDeleteResponse);
}

async function deleteAllCanvases() {
    clearNode(canvasDeleteAllResponse);
    let response = await fetch(ORIGIN + CANVAS_ENDPOINT + DELETE_ALL, {
        method: "delete"
    });

    if (!response.ok) {
        addText(canvasDeleteAllResponse, "Operation denied.");
        addClearButton(canvasDeleteAllResponse);
        return;
    }

    addText(canvasDeleteAllResponse, "Canvases removed.");
    addClearButton(canvasDeleteAllResponse);
}
//#endregion

let userGetForm = document.getElementById("user-get-form");
userGetForm.addEventListener("submit", function(e) { getUser(extractDict(e)) })
let userGetResponse = document.getElementById("user-get-response");
clearNode(userGetResponse);

let userGetAllForm = document.getElementById("user-get-all-button");
userGetAllForm.addEventListener("click", getAllUsers)
let userGetAllResponse = document.getElementById("user-get-all-response");
clearNode(userGetAllResponse);

let userPostForm = document.getElementById("user-post-form");
userPostForm.addEventListener("submit", function (e) { postUser(extractDict(e)) })
let userPostResponse = document.getElementById("user-post-response");
clearNode(userPostResponse);

let userPutForm = document.getElementById("user-put-form");
userPutForm.addEventListener("submit", function (e) { putUser(extractDict(e)) })
let userPutResponse = document.getElementById("user-put-response");
clearNode(userPutResponse);

let userDeleteForm = document.getElementById("user-delete-form");
userDeleteForm.addEventListener("submit", function (e) { deleteUser(extractDict(e)) })
let userDeleteResponse = document.getElementById("user-delete-response");
clearNode(userDeleteResponse);

let userDeleteAllButton = document.getElementById("user-delete-all-button");
userDeleteAllButton.addEventListener("click", deleteAllUsers)
let userDeleteAllResponse = document.getElementById("user-delete-all-response");
clearNode(userDeleteAllResponse);

let canvasGetForm = document.getElementById("canvas-get-form");
canvasGetForm.addEventListener("submit", function (e) { getCanvas(extractDict(e)) })
let canvasGetResponse = document.getElementById("canvas-get-response");
clearNode(canvasGetResponse);

let canvasGetAllForm = document.getElementById("canvas-get-all-button");
canvasGetAllForm.addEventListener("click", getAllCanvases)
let canvasGetAllResponse = document.getElementById("canvas-get-all-response");
clearNode(canvasGetAllResponse);

let canvasPostForm = document.getElementById("canvas-post-form");
canvasPostForm.addEventListener("submit", function (e) { postCanvas(extractDict(e)) })
let canvasPostResponse = document.getElementById("canvas-post-response");
clearNode(canvasPostResponse);

let canvasPutForm = document.getElementById("canvas-put-form");
canvasPutForm.addEventListener("submit", function (e) { putCanvas(extractDict(e)) })
let canvasPutResponse = document.getElementById("canvas-put-response");
clearNode(canvasPutResponse);

let canvasDeleteForm = document.getElementById("canvas-delete-form");
canvasDeleteForm.addEventListener("submit", function (e) { deleteCanvas(extractDict(e)) })
let canvasDeleteResponse = document.getElementById("canvas-delete-response");
clearNode(canvasDeleteResponse);

let canvasDeleteAllButton = document.getElementById("canvas-delete-all-button");
canvasDeleteAllButton.addEventListener("click", deleteAllCanvases)
let canvasDeleteAllResponse = document.getElementById("canvas-delete-all-response");
clearNode(canvasDeleteAllResponse);