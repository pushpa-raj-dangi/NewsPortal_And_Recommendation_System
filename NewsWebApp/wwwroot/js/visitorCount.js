window.onload = function () {
    let onlineCount = document.querySelector('span.online-count');
    let userActivity = document.getElementById("user_activity");
    console.log(onlineCount);
    let updateCountCallback = function (message) {
        if (!message) return;
        console.log('updateCount = ' + message);
        if (onlineCount) onlineCount.innerText = message;
    };


    let updateUrl = function (message) {
        userActivity.innerHTML += `<li style="list-style:none"><a class="text-primary" href="${message}">${message}</a></li>`;
        console.log(message)
    }

    let ab = function (msg) {
        console.log("don"+msg)
    }
    function onConnectionError(error) {
        if (error && error.message) console.error(error.message);
    }

    let countConnection = new signalR.HubConnectionBuilder().withUrl('/online').build();
    var pn = window.location.pathname;
    let urlConnection = new signalR.HubConnectionBuilder().withUrl('/url?pagename=' + pn).build();
    countConnection.on('updateCount', updateCountCallback);

    urlConnection.on('getUrl', updateUrl);
    urlConnection.onclose(ab)

    countConnection.onclose(onConnectionError);
    urlConnection.start().then(function (message) {
        console.log(message)
    }).catch(function () {
        console.log("something went wrong")
    })

    countConnection.start()
        .then(function () {
            console.log('OnlineCount Connected');
        })
        .catch(function (error) {
            console.error(error.message);
        });



};

