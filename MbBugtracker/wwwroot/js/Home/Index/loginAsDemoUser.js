function loginAsDemoUser() {

    let postData = {
        "email": "demouser@email.com",
        "password": "demopassword",
        "rememberMe": false,
        "returnUrl": null
    }


    $.ajax({
        url: "/Home/LoginUserWithAjax",
        method: "POST",
        data: JSON.stringify(postData),
        dataType: "json",
        contentType: 'application/json',
    })
        .done(() => {
            
        })
        .fail(er => {
            if (er.status == 200) {
                window.location.reload();
            }
        });

}