// 壓縮: https://skalman.github.io/UglifyJS-online/

// Vue Initialize
var sidebarMenu = new Vue({
    el: '#customMenu',
    data: {
        active_li: 0 // 預設Menu為"排班"
    },
    methods: {
        activate: function (li) {
            this.active_li = li;
        }
    }
});

// Initialize
$(() => {

    // 登出
    SignOut();

});

// 回傳是否驗證通過
let ValidateMethod = () => {
    return ValidateToken();
};

// 驗證localStorage['token']
let ValidateToken = () => {
    var vResult;
    // 判斷有無 token，沒有直接跳轉回登入頁面
    if (localStorage.getItem('token') == null) {
        ResetLocalStorage('Timeout');
        vResult = false;
    } else {
        $.ajax({
            url: '/api/JwtToken',
            type: 'GET',
            headers: { "Authorization": 'Bearer ' + localStorage.getItem('token') },
            async: false, // 不使用非同步
            success: () => { // 就是單純成功，無回傳值。
                if (ValidateAuthCookie()) { // 驗證 AuthCookie  
                    vResult = true;
                } else {
                    vResult = false;
                }
            },
            error: (data) => {
                ResetLocalStorage('AccountError');
                vResult = false;
            }
        });
    }
    return vResult;
};

// 驗證Cookie['WssAuth']
let ValidateAuthCookie = () => {
    var vResult;
    if (getCookie('WssAuth').trim() == '') {
        ResetLocalStorage('Timeout');
        vResult = false;
    } else {
        $.ajax({
            url: '/LoginService/AuthCookieCheck',
            type: 'GET',
            async: false,
            success: (data) => {
                if (data.Status == 200) {
                    SetUserInfo(); // 設置用戶訊息      
                    vResult = true;
                } else {
                    ResetLocalStorage('Timeout');
                    vResult = false;
                }
            },
            error: (data) => {
                ResetLocalStorage('AccountError');
                vResult = false;
            }
        });
    }
    return vResult;    
};



var SignOut = () => {
    $('#btn_signOut').click(() => {
        ResetLocalStorage('Logout');
    });
};

// 
var GetUserInfo = () => {
    var authUserInfo;
    $.ajax({
        url: '/ShiftService/GetUserInfo',
        type: 'GET',
        async: false, // 不使用非同步
        success: (data) => {
            if (data.Status == 200) {
                // success todo...    
                authUserInfo = data.Data;
            } else {
                ResetLocalStorage('Timeout');
            }
        },
        error: (data) => {
            ResetLocalStorage('AccountError');
        }
    });
    return authUserInfo;
};

var SetUserInfo = () => {
    $('#navbarUserName').text(GetUserInfo().name);
};


// 重置localStorage
var ResetLocalStorage = (state) => {
    switch (state) {
        case 'Timeout':
            alert('登入逾時，請重新登入!');
            break;
        case 'Logout':
            alert('帳號已登出!');
            break;
        case 'AccountError':
            alert('帳戶異常，請重新登入!');
            break;
        default:
            break;
    }
    localStorage.removeItem('token');
    window.location.href = '/login/index';
};






