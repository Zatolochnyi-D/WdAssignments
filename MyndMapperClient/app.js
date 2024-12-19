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

//#region UserMethods
async function getUser(formData) {
    clearNode(userGetResponse);
    let targetId = formData["id"];
    let response = await fetch(ORIGIN + USER_ENDPOINT + GET + targetId);

    if (!response.ok) {
        addText(userGetResponse, "User not found.");
        return;
    }

    let data = await response.json();
    let user = as(UserGetDto, data);
    addText(userGetResponse, "User info: ");
    addList(userGetResponse, ["id: " + user.id, "name: " + user.name, "email: " + user.email, "password: " + user.password, "created canvases: " + user.createdCanvases]);
}

async function getAllUsers() {
    let response = await fetch(ORIGIN + USER_ENDPOINT + GET_ALL);

    if (!response.ok) {
        console.log("Something wrong happened");
        return;
    }

    let data = await response.json();
    let convertedData = [];
    for (i in data) {
        convertedData.push(as(UserGetDto, data[i]));
    }
    console.log(convertedData);
}

async function postUser() {
    let body = new UserPostDto;
    body.name = "";
    body.email = "elderOne@wizards.mgc";
    body.password = "foolmoon";

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
        console.log(data);
        return;
    }

    console.log("Done");
}

async function putUser() {
    let body = new UserPutDto;
    body.id = 12;
    body.name = "SJDFKJEFJKWEF";
    body.email = "FSLDNFDSnlf@wizards.mgc";
    body.password = "SfKLWEfukwefkwEKFJ";

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
        console.log(data);
        return;
    }

    console.log("Done");
}

async function deleteUser() {
    let response = await fetch(ORIGIN + USER_ENDPOINT + DELETE + 12 /*user id*/, {
        method: "delete"
    });

    if (!response.ok) {
        console.log("Something wrong happened");
        return;
    }
}

async function deleteAllUsers() {
    let response = await fetch(ORIGIN + USER_ENDPOINT + DELETE_ALL, {
        method: "delete"
    });

    if (!response.ok) {
        console.log("Something wrong happened");
        return;
    }
}
//#endregion

//#region CanvasMethods
async function getCanvas() {
    
    let response = await fetch(ORIGIN + CANVAS_ENDPOINT + GET);

    if (!response.ok) {
        
        return;
    }

    let data = await response.json();
    let convertedData = as(CanvasGetDto, data);
    addText(userGetResponse, "User:");
}

async function getAllCanvases() {
    let response = await fetch(ORIGIN + CANVAS_ENDPOINT + GET_ALL);

    if (!response.ok) {
        console.log("Something wrong happened");
        return;
    }

    let data = await response.json();
    let convertedData = [];
    for (i in data) {
        convertedData.push(as(CanvasGetDto, data[i]));
    }
    console.log(convertedData);
}

async function postCanvas() {
    let body = new UserPostDto;
    body.name = "";
    body.email = "elderOne@wizards.mgc";
    body.password = "foolmoon";

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
        console.log(data);
        return;
    }

    console.log("Done");
}

async function putCanvas() {
    let body = new UserPutDto;
    body.id = 12;
    body.name = "SJDFKJEFJKWEF";
    body.email = "FSLDNFDSnlf@wizards.mgc";
    body.password = "SfKLWEfukwefkwEKFJ";

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
        console.log(data);
        return;
    }

    console.log("Done");
}

async function deleteCanvas() {
    let response = await fetch(ORIGIN + CANVAS_ENDPOINT + DELETE, {
        method: "delete"
    });

    if (!response.ok) {
        console.log("Something wrong happened");
        return;
    }
}

async function deleteAllCanvases() {
    let response = await fetch(ORIGIN + CANVAS_ENDPOINT + DELETE_ALL, {
        method: "delete"
    });

    if (!response.ok) {
        console.log("Something wrong happened");
        return;
    }
}
//#endregion

let userGetForm = document.getElementById("user-get-form");
userGetForm.addEventListener("submit", function(e) { getUser(extractDict(e)) })

let userGetResponse = document.getElementById("user-get-response");
clearNode(userGetResponse);