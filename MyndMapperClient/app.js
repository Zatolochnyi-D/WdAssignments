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

//#region UserMethods
async function getUser(e) {
    e.preventDefault();
    formData = new FormData(e.target);
    formProps = Object.fromEntries(formData);
    console.log(formData);
    console.log(formProps);
    for (let [key, value] of formData.entries()) {
        console.log(key, value);
    }
    return;
    let response = await fetch(ORIGIN + USER_ENDPOINT + GET);

    if (!response.ok) {
        console.log("Something wrong happened");
        return;
    }

    let data = await response.json();
    let convertedData = as(UserGetDto, data);
    console.log(convertedData);
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
        console.log("Something wrong happened");
        return;
    }

    let data = await response.json();
    let convertedData = as(CanvasGetDto, data);
    console.log(convertedData);
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

// let userGetInput = document.getElementById("user-get-input");
// let userGetSubmit = document.getElementById("user-get-submit");
// userGetSubmit.addEventListener("click", getUser)

let userGetForm = document.getElementById("user-get-form");
userGetForm.addEventListener("submit", getUser)

// let userResponse = document.getElementById("user-response");
// userResponse.innerHTML = "";

console.log("huh?");