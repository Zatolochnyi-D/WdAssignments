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

// DTOs
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

async function getAll() {
    let canvasGetAllUrl = ORIGIN + CANVAS_ENDPOINT + GET_ALL;
    let response = await fetch(canvasGetAllUrl)
    console.log(response);
    let resp = await response.json()
    console.log(resp);
    let obj = Object.assign(new CanvasGetDto, resp[0]);
    console.log(obj);
    // if (response.ok) {
    //     let data = await response.json();
    //     console.log(data);
    //     // let array = [];
    //     // for (i = 0; i < data.lenght; i++) {
    //     //     array.push(Object.assign(new CanvasGetDto, JSON.parse(data[i])))
    //     // }
    // }
}

async function postCanvas() {
    let canvasPostUrl = ORIGIN + CANVAS_ENDPOINT + POST;
    let post = new CanvasPostDto()
    post.name = "lsdlgkdfg"
    post.ownerId = 10;
    console.log(JSON.stringify(post));
    let response = await fetch(canvasPostUrl, {
        method: "post",
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(post),
    })
    console.log(response);
}

let button = document.getElementById("test-button");
button.addEventListener("click", postCanvas)