$(document).on("click", ".modal-btn", function (e) {
    e.preventDefault();
    let url = $(this).attr("href");
    fetch(url).then(response => {
        if (response.ok) {
            return response.text()
        }
        else {
            alert("Xeta bas verdi")
            return
        }
    })
        .then(data => {
            $("#quick_view .modal-content").html(data)

        })
        .then(() => {
            console.log("bitdi")
            $("#quick_view").modal("show")
        })

})

$(document).on("click", ".basket-add-btn", function (e) {
    e.preventDefault();

    let url = $(this).attr("href");
    fetch(url).then(response => {
        if (!response.ok) {
            alert("xeta bas verdi")
        }
        else return response.text()
    }).then(data => {
        $(".minicart-inner-content").html(data)
    }).then(() => {
        fetch('/home/GetBasketCount')
            .then(response => response.json())
            .then(data => {
                const countElement = document.getElementById('basket-count');
                countElement.innerHTML = data.count;
            })
            .catch(error => {
                console.error('İstek sırasında bir hata oluştu:', error);
            });
    })
})

$(document).on("click", ".minicart-close, .offcanvas-close, .offcanvas-overlay", function () {
    $("body").removeClass('fix');
    $(".offcanvas-search-inner, .minicart-inner").removeClass('show')
})

$(document).on("click", ".remove-btn", function (e) {
    e.preventDefault();
    let url = $(this).attr("href");
    fetch(url).then(response => {
        if (!response.ok) {
            alert("xeta bas verdi")
        }
        else return response.text()
    }).then(data => {
        $(".minicart-inner-content").html(data)
    }).then(() => {
        fetch('/home/GetBasketCount')
            .then(response => response.json())
            .then(data => {
                const countElement = document.getElementById('basket-count');
                countElement.innerHTML = data.count;
            })
            .catch(error => {
                console.error('İstek sırasında bir hata oluştu:', error);
            });
    })
})

