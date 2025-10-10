let btnLogin = document.querySelector('#btn-login');

function login(dataLogin) {
    return fetch('api/Auth', {
        method: 'POST',
        headers: {
            "Content-Type": 'application/json'
        },
        body: JSON.stringify(dataLogin)
    })
        .then(response => {
            if (!response.ok) {
                return response.json().then(err => Promise.reject(err));
            }
            return response.json();
        });
}

btnLogin.addEventListener('click', () => {
    const username = document.querySelector('#username')?.value || "charly";
    const password = document.querySelector('#password')?.value || "12334";

    login({ usuarioname: username, password: password })
        .then(resp => {
            console.log(resp)
            if (resp.success) {
                console.log('Login exitoso, redirigiendo...');
                window.location.href = resp.redirectUrl;
            }
        })
        .catch(err => {
            console.error('Error en login:', err);
            alert(err.message || 'Credenciales inválidas');
        });
});

