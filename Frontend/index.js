function $(id) {
    return document.getElementById(id);
}

console.log("Start");

function isValidMobileNo(mobileNo) {
    const pattern = /^\d{10}$/;
    return pattern.test(mobileNo);
}

$('btnenter').onclick = async function() {
    var mobileNo = $('mobile').value;
    var password = $('password').value;
    if (isValidMobileNo(mobileNo)) {
        try {
            // Check if the mobile number exists
            const response = await fetch(`https://localhost:7257/api/UsersApi`, {
                method: 'GET',
                headers: {
                    'Content-Type': 'application/json',
                }
            });

            if (!response.ok) {
                throw new Error('Network response was not ok');
            }

            const data = await response.json();
            console.log(data);

            let userId = null;

            // Check if the mobile number exists in the list of objects
            data.forEach(user => {
                console.log(user);
                
                if (user.mobileNumber === mobileNo && user.password===password) {
                    console.log("In for Each");
                    console.log(userId);
                    userId = user.userId;
                    console.log(userId);
                    
                }
            });
            console.log(userId);
            
            if (!userId) {
                // Mobile number does not exist, proceed to POST request
                const postResponse = await fetch('https://localhost:7257/api/UsersApi', {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json',
                    },
                    body: JSON.stringify({ mobileNumber: mobileNo,password:password })
                });
                console.log(postResponse.ok);
                
                if (!postResponse.ok) {
                    throw new Error('Failed to add new mobile number');
                }

                const newUser = await postResponse.json();
                userId = newUser.userId; // Assuming the API returns the new user object with UserId
            }
            else{
                alert("Already Signed Up,Now Login!")
            }
            // Store the userId in localStorage regardless of whether it's new or existing
            localStorage.setItem('userId', userId);

            // Redirect to burger.html
            //window.location.href = 'burger.html';
        } catch (error) {
            console.error('Error:', error);
            alert('There was an error processing your request. Please try again.');
        }
        
        //console.log(burgerBg);
        
    } else {
        alert('Enter valid mobile number');
    }
}

document.getElementById('btnSignIn').addEventListener('click', async () => {
    const mobileNumber = document.getElementById('mobile').value; 
    const password = document.getElementById('password').value;

    const requestData = {
        MobileNumber: mobileNumber,
        Password: password,
        
    };
    console.log("clicked on sign in");
    
    try {
        const response = await fetch('https://localhost:7257/api/Auth/login', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(requestData)
        });

        if (!response.ok) {
            throw new Error('Login failed: ' + response.statusText);
        }

        const data = await response.json();
        document.cookie = `token=${data.token}; path=/`; // Store token in cookies

        // Check if the user is admin
        if (mobileNumber === '8421554421' && password === 'admin') {
            alert('Login successful! Welcome, Admin!');
            
            window.location.href = 'admin-dashboard.html';
        } else {
            
            window.location.href = 'burger.html';
        }

    } catch (error) {
        console.error('Error:', error);
        alert('Login failed. Please check your credentials and try again.');
    }
    var burgerBg = $('burgerbg');
    burgerBg.style.translate=('-100%');
    burgerBg.style.transition='3s';
});

