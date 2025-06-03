const books = [
  {
    title: "Искусство войны",
    author: "Сунь-Цзы",
    price: 13,
    oldPrice: 15,
    image: "bookPics/war-art.webp"
  },
  {
    title: "48 Законов власти",
    author: "Роберт Грин",
    price: 12,
    oldPrice: 14,
    image: "bookPics/Без названия.jpeg"
  },
  {
    title: "48 Законов власти",
    author: "Роберт Грин",
    price: 12,
    oldPrice: 14,
    image: "bookPics/Без названия.jpeg"
  },
  {
    title: "48 Законов власти",
    author: "Роберт Грин",
    price: 12,
    oldPrice: 14,
    image: "bookPics/Без названия.jpeg"
  },
  {
    title: "Икигай",
    author: "Франсеск Миральес",
    price: 9.9,
    oldPrice: 11.9,
    image: "bookPics/ikigay.jpeg"
  }
];
const cart=[];


function scrollToBooks(){
  const title=document.getElementById("new-books-id");
  const top=title.offsetTop-100;

  window.scrollTo(
    {
      top: top,
      behavior: "smooth"
    }
  );
}

function renderBooks() {
  const cardsContainer = document.getElementById("cards-section-id");

  books.forEach(book => {
    const card = document.createElement("div");
    card.className = "book-card";

    card.innerHTML = `
      <img src="${book.image}" alt="Обложка книги">
      <h3>${book.title}</h3>
      <p class="author">${book.author}</p>
      <p class="price">${book.price} $ <span class="old-price">${book.oldPrice} $</span></p>
      <button class="add-to-cart" data-title="${book.title}">В корзину</button>
`;

    cardsContainer.appendChild(card);
  }
  );
  const buttons = document.querySelectorAll(".add-to-cart");

  buttons.forEach(button => {
    button.addEventListener("click", () => {
      const title = button.getAttribute("data-title");
      const bookToAdd = books.find(b => b.title === title);

      if (bookToAdd) {
        cart.push(bookToAdd);
        console.log("Добавлено в корзину:", bookToAdd);
        updateCartCount();
      }
    });
  });
}

function updateCartCount(){
  const countSpan=document.getElementById("cart-num");
  countSpan.textContent=cart.length;
}

const cartPopup=document.getElementById("cart-popup");
const cartItemsList=document.getElementById("cart-items");
const cartButton=document.getElementById("cart");
const closeCartBtn=document.getElementById("close-cart");

cartButton.addEventListener("click",() => {
  renderCartItems();
  cartPopup.classList.add("open");
});

closeCartBtn.addEventListener("click",()=>{
  cartPopup.classList.remove("open");
});

function renderCartItems(){
  cartItemsList.innerHTML="";
  cart.forEach(book =>{
    const li=document.createElement("li");
    li.textContent=`${book.title} — ${book.price} $`;
    cartItemsList.appendChild(li);
  });
}

document.addEventListener("DOMContentLoaded",() =>{
  renderBooks();
  updateCartCount();
});