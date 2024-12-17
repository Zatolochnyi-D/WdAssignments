class CanvasGetDto {
    Id
    Name
    CreationDate
    OwnerId
}

class CanvasPostDto {
    OwnerId
    Name
}

class CanvasPutDto {
    Id
    Name
}

async function getAll() {
    let canvasGetAllUrl = "http://localhost:5110/canvases/get/all";
    let response = await fetch(canvasGetAllUrl, {
        method: "GET",
        mode: "no-cors"
    })
    console.log(response);
    if (response.ok) {
        let data = await response.json();
        console.log(data);
        // let array = [];
        // for (i = 0; i < data.lenght; i++) {
        //     array.push(Object.assign(new CanvasGetDto, JSON.parse(data[i])))
        // }
    }
}

getAll()