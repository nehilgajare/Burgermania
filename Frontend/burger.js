let burgers = []; // Initialize burgers as an empty array

function getCookie(name) {
    const value = `; ${document.cookie}`;
    const parts = value.split(`; ${name}=`);
    if (parts.length === 2) return parts.pop().split(';').shift();
}

// Fetch the JWT token from cookies
const authToken = getCookie('token');
// Function to fetch burgers from API
async function fetchBurgers() {
    const heading = document.getElementById('heading');
    heading.style.transform = 'translateX(100%,-50%)';
    heading.style.transition = '1s';

    try {
        const response = await fetch('https://localhost:7257/api/BurgersApi', {
            method: 'GET',
            headers: {
                'Content-Type': 'application/json', 
                'Authorization': `Bearer ${authToken}` 
            }
        });

        if (!response.ok) {
            throw new Error('Network response was not ok');
        }

        burgers = await response.json(); 
        // console.log(burgers);
        
        // You can add code here to update the UI with the fetched burgers

    } catch (error) {
        console.error('Error fetching burgers:', error);
    }
}

// Function to display burgers based on category
function displayBurgers(category) {
   
    var cardGrid = document.getElementById('card-grid');
    cardGrid.innerHTML = ''; // Clear previous content
    burgers.filter(burger => burger.category === category).forEach((burger, index) => {
        var card = document.createElement('div');
        card.className = 'card';
        // Use the burger's name or index to create a unique ID for each quantity input
        var quantityId = 'quantity-' + burger.name.replace(/\s+/g, '-').toLowerCase();
       
        card.innerHTML = `
            <img src="${burger.imageLink}" alt="${burger.name}">
            <div class="card-content">
                <h4><b>${burger.name}</b></h4>
                <p>Price: Rs${burger.price}</p>
                <label for="${quantityId}">Quantity</label>
                <input type="number" id="${quantityId}" name="quantity" min="1" value="1"/>
                <button onclick="addToCart('${burger.name}', ${burger.price}, document.getElementById('${quantityId}').value, '${quantityId}')">Add to Cart</button>
            </div>
        `;
        cardGrid.appendChild(card);
    });
}    

document.getElementById('burger').addEventListener('click', function () {

    displayBurgers(this.value);
});

function addToCart(itemName, price, quantity, quantityId) {
    var numericQuantity = parseInt(quantity, 10);
    var userId = localStorage.getItem('userId');

    if (!userId) {
        alert("User ID not found. Please log in.");
        return;
    }

    var cart = JSON.parse(localStorage.getItem('cart')) || [];
    
    // Find the burger object that matches the itemName
    var burgerItem = burgers.find(burger => burger.name === itemName);
    
    if (!burgerItem) {
        console.error('Burger not found:', itemName);
        return;
    }

    var existingItem = cart.find(item => item.name === itemName);
    if (existingItem) {
        existingItem.quantity += numericQuantity;
    } else {
        cart.push({ 
            name: itemName, 
            price: price, 
            quantity: numericQuantity, 
            burgerId: burgerItem.burgerId, 
            userId: userId 
        });
    }

    localStorage.setItem('cart', JSON.stringify(cart));
    alert(numericQuantity + " - " + itemName + " added to cart.");

    document.getElementById(quantityId).value = 1;
}



document.getElementById('view-my-cart').onclick = function(){
    window.location.href ='view-my-cart.html'
}

// Call fetchBurgers when the page loads
// document.addEventListener('DOMContentLoaded',)
document.addEventListener('DOMContentLoaded', fetchBurgers);