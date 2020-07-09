function typeAheadForEditForm() {

    // bloodhound for typeahead
    let appUsers = new Bloodhound({
        datumTokenizer: Bloodhound.tokenizers.obj.whitespace('value'),
        queryTokenizer: Bloodhound.tokenizers.whitespace,
        remote: {
            url: '/api/users?userName=%QUERY',
            wildcard: '%QUERY'
        }
    });

    //typeahead box
    $("#selectUsers").typeahead({
        hint: true,
        highlight: true,
        minLength: 1
    }, {
        name: 'users',
        display: "userName",
        source: appUsers,
        limit: 10
    }).on("typeahead:select", function (e, user) {

        if (selectedUserIds.includes(user.id)) {
            $("#selectUsers").typeahead("val", "");
            toastr.error("Already added!");
            return;
        }

        let listItem = document.createElement("li")
        let removeButton = document.createElement("button")
        listItem.classList.add("d-flex", "p-2", "justify-content-between", "align-items-center")
        listItem.innerText = user.userName;
        removeButton.setAttribute("data-id", user.id)
        removeButton.setAttribute("data-userName", user.userName)
        removeButton.classList.add("btn", "btn-danger", "removeBtn")
        removeButton.innerText = "Remove";

        listItem.appendChild(removeButton);

        $("#selectedDevelopers").append(listItem);

        selectedUserIds.push(user.id);

        $("#selectUsers").typeahead("val", "");
    });
}