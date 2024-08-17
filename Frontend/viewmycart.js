// Function to update the display of the cart
function updateCartDisplay() {
    const cartItems = JSON.parse(localStorage.getItem('cart')) || [];
    console.log(cartItems);
    let totalQuantity = 0;
    let totalPrice = 0;

    const tableBody = document.getElementById('cart-table').getElementsByTagName('tbody')[0];
    tableBody.innerHTML = ''; // Clear the table

    // Create rows for each item in the cart
    cartItems.forEach((item, index) => {
        const row = tableBody.insertRow();
        row.insertCell(0).textContent = item.name;
        row.insertCell(1).textContent = item.price;
        row.insertCell(2).textContent = item.quantity;
        const removeCell = row.insertCell(3);
        removeCell.innerHTML = '<button onclick="removeItem(' + index + ')">Remove</button>';

        // Update total quantity and price
        totalQuantity += item.quantity;
        totalPrice += item.price * item.quantity;
    });

    // Add total row
    const totalRow = tableBody.insertRow();
    totalRow.insertCell(0).textContent = 'Total';
    totalRow.insertCell(1).textContent = totalPrice;
    totalRow.insertCell(2).textContent = totalQuantity;
    totalRow.insertCell(3);

    // Update the totals display
    document.getElementById('total-quantity').textContent = totalQuantity;
    document.getElementById('total-price').textContent = totalPrice;

    // Calculate and display discount
    const discount = calculateDiscount(totalPrice);
    document.getElementById('discount').textContent = discount.percentage;
    document.getElementById('total-price-after-discount').textContent = discount.totalAfterDiscount;
}



// Function to remove an item from the cart
function removeItem(index) {
    const cartItems = JSON.parse(localStorage.getItem('cart')) || [];
    cartItems.splice(index, 1); // Remove the item at the specified index
    localStorage.setItem('cart', JSON.stringify(cartItems)); // Update local storage
    updateCartDisplay(); // Update the display
}

// Function to calculate discount
function calculateDiscount(totalPrice) {
    let discountPercentage = 0;
    if (totalPrice >= 500 && totalPrice < 1000) {
        discountPercentage = 5;
    } else if (totalPrice >= 1000) {
        discountPercentage = 10;
    }
    const totalAfterDiscount = totalPrice - (totalPrice * discountPercentage / 100);
    return { percentage: discountPercentage, totalAfterDiscount: totalAfterDiscount.toFixed(2) };
}

// Function to handle the place order button
async function placeOrder() {
    const totalPrice = parseFloat(document.getElementById('total-price').textContent);
    const discount = calculateDiscount(totalPrice);
    const totalQuantity = parseInt(document.getElementById('total-quantity').textContent);
    const userId = localStorage.getItem('userId');
    const cart = JSON.parse(localStorage.getItem('cart')) || [];

    if (!userId) {
        alert("User ID not found. Please log in.");
        return;
    }

    if (cart.length === 0) {
        alert("Your cart is empty. Please add items before placing an order.");
        return;
    }

    try {
        for (const item of cart) {
            const orderItem = {
                burgerId: item.burgerId,
                itemName: item.name,
                quantity: item.quantity,
                price: item.price,
                userId: parseInt(userId),
                  // Ensure userId is a number
            };

            console.log('Sending order item:', orderItem); // Debugging line

            const response = await fetch('https://localhost:7257/api/OrdersApi', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify(orderItem)
            });

            if (!response.ok) {
                const errorText = await response.text();
                throw new Error(`Failed to place order for item ${item.name}: ${errorText}`);
            }

            const result = await response.json();
            console.log('Order item placed successfully:', result);
        }

        // Clear the cart after all items have been successfully ordered
        localStorage.removeItem('cart');

        alert('Order placed successfully!\n' +
            'Total Quantity: ' + totalQuantity +
            '\nTotal Price: Rs. ' + totalPrice +
            '\nDiscount: ' + discount.percentage + '%' +
            '\nTotal Price after Discount: Rs. ' + discount.totalAfterDiscount + '/-');

        // Optionally, redirect to a confirmation page or refresh the current page
        // window.location.href = 'order-confirmation.html';
    } catch (error) {
        console.error('Error placing order:', error);
        alert('Failed to place order. Please try again.');
    }
}

// Call updateCartDisplay on page load to show the cart items
document.addEventListener('DOMContentLoaded', updateCartDisplay);
