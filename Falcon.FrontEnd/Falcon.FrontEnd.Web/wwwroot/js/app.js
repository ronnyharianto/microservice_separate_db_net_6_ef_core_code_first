// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

let token = localStorage.getItem("token");

if (token == null) {
    var data = {
        username: "username",
        password: "password"
    }

    $.ajax({
        headers: {
            "Content-Type": "application/json"
        },
        url: glbFalconUrl + "security/login",
        type: "POST",
        data: JSON.stringify(data),
        success: function (r) {
            localStorage.setItem("token", r.obj);
            SetupAjax(r.obj);
        }
    });
}
else {
    SetupAjax(token);
}

function SetupAjax(t) {
    $.ajaxSetup({
        headers: {
            "Authorization": "Bearer " + t,
            "Content-Type": "application/json"
        },
        dataType: "json"
    });
}

//eyJhbGciOiJIUzUxMiIsInR5cCI6IkpXVCJ9.eyJJZCI6ImIwYmUwMjhlLWIyNzUtNDczYy05MzgwLTkwNmIyMTM4NTExZSIsInN1YiI6InVzZXJuYW1lIiwiZW1haWwiOiJ1c2VybmFtZSIsImp0aSI6ImIyMGU0YmIzLWJhMGItNDYzMC1hYjYyLTY1YjNiMWRjZWNiYyIsIm5iZiI6MTY4NjcyMTMzOSwiZXhwIjoxNjg2ODA3NzM5LCJpYXQiOjE2ODY3MjEzMzksImlzcyI6Iklzc3VlciIsImF1ZCI6IkF1ZGllbmNlIn0.nc7v560UW5Z28T775nGA2eNvMdBmhYlNrmWxKKOAYvp4Y01PDaD7G9pBfygqAFpTuksNxJeDdJNR_L6JaTCAcg