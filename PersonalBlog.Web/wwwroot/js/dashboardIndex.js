$(document).ready(function () {
    var yearlyArticleUrl = app.Urls.yearlyArticleUrl;
    var totalArticleCountUrl = app.Urls.totalArticleCountUrl;
    var totalCategoryCountUrl = app.Urls.totalCategoryCountUrl;
    var totalUserCountUrl = app.Urls.totalUserCountUrl;
    var activeUserCountUrl = app.Urls.activeUserCountUrl;

    $.ajax({
        type: "GET",
        url: totalArticleCountUrl,
        dataType: "json",
        success: function (data) {
            $("h3#totalArticleCount").append(data);
        },
        error: function () {
            toastr.error("Makale analizleri yüklenirken hata oluştu.", "Hata");
        }
    });



    $.ajax({
        type: "GET",
        url: totalCategoryCountUrl,
        dataType: "json",
        success: function (data) {
            $("h3#totalCategoryCount").append(data);
        },
        error: function () {
            toastr.error("Kategori analizleri yüklenirken hata oluştu1.", "Hata");
        }
    });

    $.ajax({
        type: "GET",
        url: totalUserCountUrl,
        dataType: "json",
        success: function (data) {
            $("h3#totalUserCount").append(data);
        },
        error: function () {
            toastr.error("Kullanıcı analizleri yüklenirken hata oluştu.", "Hata");
        }
    });

    $.ajax({
        type: "GET",
        url: activeUserCountUrl,
        dataType: "json",
        success: function (data) {
            $("h3#totalActiveUserCount").append(data);
        },
        error: function () {
            toastr.error("Kullanıcı analizleri yüklenirken hata oluştu.", "Hata");
        }
    });

    $.ajax({
        type: "GET",
        url: yearlyArticleUrl,
        dataType: "json",
        success: function (count) {
            const ctx = document.getElementById('myChart');

            new Chart(ctx, {
                type: 'bar',
                data: {
                    labels: ['Ocak', 'Subat', 'Mart', 'Nisan', 'Mayis', 'Haziran', 'Temmuz', 'Agustos', 'Eylul', 'Ekim', 'Kasim', 'Aralik'],
                    datasets: [{
                        label: 'Makale',
                        data: count,
                        borderWidth: 1
                    }]
                },
                options: {
                    scales: {
                        y: {
                            beginAtZero: true
                        }
                    }
                }
            });
        },
        error: function () {
            toastr.error("Kullanıcı analizleri yüklenirken hata oluştu.", "Hata");
        }
    });






});


