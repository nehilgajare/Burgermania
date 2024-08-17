const apiUrl = 'https://localhost:7257/api/BurgersApi';

// Function to get the JWT token from cookies
function getCookie(name) {
    const value = `; ${document.cookie}`;
    const parts = value.split(`; ${name}=`);
    if (parts.length === 2) return parts.pop().split(';').shift();
}


const authToken = getCookie('token'); 

document.addEventListener('DOMContentLoaded', fetchBurgers);

async function fetchBurgers() {
    try {
        const response = await fetch(apiUrl, {
            headers: {
                'Authorization': `Bearer ${authToken}` // Include the JWT token in the Authorization header
            }
        });

        if (!response.ok) {
            throw new Error(`HTTP error ${response.status}`);
        }

        const burgers = await response.json();
        const burgerCollection = document.getElementById('burgerCollection');
        burgerCollection.innerHTML = '';

        burgers.forEach(burger => {
            const li = document.createElement('li');
            // Display burger name, price, category, and image
            li.innerHTML = `
                <strong>${burger.name}</strong> - $${burger.price} 
                <span>(Category: ${burger.category})</span>
                <img src="${burger.imageLink}" alt="${burger.name}" style="width: 50px; height: auto; margin-left: 10px;">
            `;

            const deleteButton = document.createElement('button');
            deleteButton.textContent = 'Delete';
            deleteButton.classList.add('delete-btn');
            deleteButton.onclick = () => deleteBurger(burger.burgerId);
            li.appendChild(deleteButton);

            const updateButton = document.createElement('button');
            updateButton.textContent = 'Update';
            updateButton.classList.add('update-btn');
            updateButton.onclick = () => updateBurger(burger.burgerId,burger.imageLink);
            li.appendChild(updateButton);

            burgerCollection.appendChild(li);
        });
    } catch (error) {
        console.error('Error fetching burgers:', error);
        // Optionally display an error message to the user
    }
}

async function updateBurger(id, imageLink) {
    const newName = prompt('Enter new name for the burger:');
    const newPrice = prompt('Enter new price for the burger:');
    const newCategory = prompt('Enter new category for the burger:');
    // const categoryDropdown = document.getElementById('burgerCategory');
    // const newCategory = categoryDropdown.options[categoryDropdown.selectedIndex].value;

    const updatedBurger = {
        BurgerId: id, 
        Name: newName,
        Category: newCategory,
        Price: parseFloat(newPrice), 
        ImageLink: imageLink 
    };
    console.log(updatedBurger);
    
    try {
        const response = await fetch(`${apiUrl}/${id}`, {
            method: 'PUT',
            headers: {
                'Content-Type': 'application/json',
                'Authorization': `Bearer ${authToken}` // Include the JWT token for update as well
            },
            body: JSON.stringify(updatedBurger)
        });
        console.log(response);
        
        if (response.ok) {
            console.log("In response ok");
            fetchBurgers();
            console.log("After fetch");
            
        } else {
            const errorData = await response.json();
            console.error('Failed to update burger:', errorData);
            alert('Failed to update burger');
        }
    } catch (error) {
        console.error('Error updating burger:', error);
        // Optionally display an error message to the user
    }
}
 


document.getElementById('burgerForm').onsubmit = async (event) => {
    event.preventDefault(); 
    await addBurger(); 
};

let isAdding = false; 

async function addBurger() {
    if (isAdding) return; 
    isAdding = true;

    const name = document.getElementById('burgerName').value;
    const category = document.getElementById('burgerCategory').value;
    const price = parseFloat(document.getElementById('burgerPrice').value);
    const imagelink = document.getElementById('burgerImagelink').value;

    // Validate inputs
    if (name && category && !isNaN(price) && imagelink) {
        try {
            const response = await fetch(apiUrl, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    'Authorization': `Bearer ${authToken}`
                },
                body: JSON.stringify({ name, category, price, imagelink })
            });

            if (response.ok) {
                // Clear input fields after successful addition
                document.getElementById('burgerName').value = '';
                document.getElementById('burgerCategory').value = '';
                document.getElementById('burgerPrice').value = '';
                document.getElementById('burgerImagelink').value = '';
                fetchBurgers(); // Refresh the burger list
            } else {
                alert('Failed to add burger');
            }
        } catch (error) {
            console.error('Error adding burger:', error);
            alert('An error occurred while adding the burger.'); // Inform the user
        }
    } else {
        alert('Please fill in all fields correctly');
    }
    
    isAdding = false; // Reset after processing
}


async function deleteBurger(id) {
    try {
        const response = await fetch(`${apiUrl}/${id}`, {
            method: 'DELETE',
            headers: {
                'Authorization': `Bearer ${authToken}` // Include the JWT token for deletion as well
            }
        });

        if (response.ok) {
            fetchBurgers();
        } else {
            alert('Failed to delete burger');
        }
    } catch (error) {
        console.error('Error deleting burger:', error);
        // Optionally display an error message to the user
    }
}