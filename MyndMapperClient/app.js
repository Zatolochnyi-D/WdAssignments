const ORIGIN = "http://localhost:5110/";
const USER_ENDPOINT = "users/";
const CANVAS_ENDPOINT = "canvases/";
const GET_ALL = "get/all"

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

let button = document.getElementById("test-button");
button.addEventListener("click", getAll)

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