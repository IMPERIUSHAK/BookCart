const btn = document.querySelector('.menu-btn');
const drawer = document.querySelector('.side-drawer');
const overlay = document.querySelector('.overlay');

function toggleMenu() {
  drawer.classList.toggle('open');
  overlay.classList.toggle('visible');
}

btn.addEventListener('click', toggleMenu);
overlay.addEventListener('click', toggleMenu);

function toNum(str) {
  const num = Number(str.replace(/ /g, ""));
  return num;
}

function toCurrency(num) {
  const format = new Intl.NumberFormat("ru-RU", {
    style: "currency",
    currency: "RUB",
    minimumFractionDigits: 0,
  }).format(num);
  return format;
}
const cardAddArr = Array.from(document.querySelectorAll(".card-btn"));
const cartNum = document.querySelector("#cart-num");
const cart = document.querySelector("#cart");
const popup = document.querySelector(".popup");
const popupClose = document.querySelector("#popup-close");
const body = document.body;
const popupContainer = document.querySelector("#popup-container");
const popupProductList = document.querySelector("#popup-product-list");
const popupCost         = document.querySelector("#popup-cost");
const popupDiscount     = document.querySelector("#popup-discount");
const popupCostDiscount = document.querySelector("#popup-cost-discount");

popupCost.textContent         = toCurrency(myCart.cost);
popupDiscount.textContent     = toCurrency(myCart.discount);
popupCostDiscount.textContent = toCurrency(myCart.costDiscount);



cart.addEventListener("click", (e) => {
  e.preventDefault();
  popupContainerFill();
  popup.classList.add("popup--open");
  body.classList.add("lock");
});

popupClose.addEventListener("click", (e) => {
  e.preventDefault();
  popup.classList.remove("popup--open");
  body.classList.remove("lock");
});

class Product {
  imageSrc;
  name;
  price;
  priceDiscount;
  constructor(card) {
    this.imageSrc = card.querySelector(".card-image").children[0].src;
    this.name = card.querySelector(".card-title").innerText;
    this.price = card.querySelector(".card-price--common").innerText;
    this.priceDiscount = card.querySelector(".card-price--discount").innerText;
  }
}

class Cart {
  products;
  constructor() {
    this.products = [];
  }
  get count() {
    return this.products.length;
  }
  addProduct(product) {
    this.products.push(product);
  }
  removeProduct(index) {
    this.products.splice(index, 1);
  }
  get cost() {
    const prices = this.products.map((product) => {
      return toNum(product.price);
    });
    const sum = prices.reduce((acc, num) => {
      return acc + num;
    }, 0);
    return sum;
  }
  get costDiscount() {
    const prices = this.products.map((product) => {
      return toNum(product.priceDiscount);
    });
    const sum = prices.reduce((acc, num) => {
      return acc + num;
    }, 0);
    return sum;
  }
  get discount() {
    return this.cost - this.costDiscount;
  }
}
const myCart = new Cart();

if (localStorage.getItem("cart") == null) {
  localStorage.setItem("cart", JSON.stringify(myCart));
}

const savedCart = JSON.parse(localStorage.getItem("cart")) || { products: [] };
myCart.products = savedCart.products;

// 2. Обновляем отображение числа товаров
cartNum.textContent = myCart.count;

// 3. Вешаем обработчики на все кнопки "В корзину"
cardAddArr.forEach((cardAdd) => {
  cardAdd.addEventListener("click", (e) => {
    e.preventDefault();

    // Определяем карточку и создаём новый продукт
    const card = e.target.closest(".card");
    const product = new Product(card);

    // Синхронизируемся с хранилищем на случай, если кто-то внёс изменения
    const currentCart = JSON.parse(localStorage.getItem("cart")) || { products: [] };
    myCart.products = currentCart.products;

    // Добавляем новый товар, сохраняем и обновляем счётчик
    myCart.addProduct(product);
    localStorage.setItem("cart", JSON.stringify(myCart));
    cartNum.textContent = myCart.count;
  });
});
function popupContainerFill() {
  console.log(
  "DEBUG cart:", myCart.products,
  "cost:", myCart.cost,
  "discount:", myCart.discount,
  "total:", myCart.costDiscount
);

  popupProductList.innerHTML = null;
  const savedCart = JSON.parse(localStorage.getItem("cart"));
  myCart.products = savedCart.products;
  const productsHTML = myCart.products.map((product) => {
    const productItem = document.createElement("div");
    productItem.classList.add("popup-product");

    const productWrap1 = document.createElement("div");
    productWrap1.classList.add("popup-product-wrap");
    const productWrap2 = document.createElement("div");
    productWrap2.classList.add("popup-product-wrap");

    const productImage = document.createElement("img");
    productImage.classList.add("popup-product-image");
    productImage.setAttribute("src", product.imageSrc);

    const productTitle = document.createElement("h2");
    productTitle.classList.add("popup-product-title");
    productTitle.innerHTML = product.name;

    const productPrice = document.createElement("div");
    productPrice.classList.add("popup-product-price");
    productPrice.innerHTML = toCurrency(toNum(product.priceDiscount));

    const productDelete = document.createElement("button");
    productDelete.classList.add("popup-product-delete");
    productDelete.innerHTML = "";

    productDelete.addEventListener("click", () => {
      const idx = myCart.products.findIndex(p => p.name === product.name);
      myCart.removeProduct(idx);
      localStorage.setItem("cart", JSON.stringify(myCart));
      popupContainerFill();
    });

    productWrap1.appendChild(productImage);
    productWrap1.appendChild(productTitle);
    productWrap2.appendChild(productPrice);
    productWrap2.appendChild(productDelete);
    productItem.appendChild(productWrap1);
    productItem.appendChild(productWrap2);

    return productItem;
  });

  productsHTML.forEach((productHTML) => {
    popupProductList.appendChild(productHTML);
  });

  popupCost.value = toCurrency(myCart.cost);
  popupDiscount.value = toCurrency(myCart.discount);
  popupCostDiscount.value = toCurrency(myCart.costDiscount);
}