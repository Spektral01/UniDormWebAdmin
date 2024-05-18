<template>
  <q-layout view="hHh lpR fFf">
    <q-page-container>
      <router-view>
        <div class="content">
          <div class="my-header">
            <q-header class="bg-primary text-white" height-hint="98" fixed>
              <q-toolbar>
                <q-toolbar-title>
                  <q-avatar>
                    <img
                      src="https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcRqJhV0BsZQGBGfxK-8EHS05JF4vdeYvN204NHOLOMg4A&s"
                    />
                  </q-avatar>
                  Магазин
                </q-toolbar-title>
              </q-toolbar>

              <q-tabs align="left">
                <q-route-tab to="/notification" label="Объявления" />
                <q-route-tab to="/shop" label="Магазин" />
                <q-route-tab to="/dormitory" label="Общежития" />
                <q-route-tab to="/user" label="Студенты" />
              </q-tabs>
            </q-header>
          </div>

          <div
            v-for="(item, index) in shopItems"
            :key="index"
            class="input-container"
          >
            <div class="q-pa-md row-horizontal-alignment">
              <div class="row justify-between">
                <div class="input-and-image">
                  <div class="input-fields">
                    <q-input
                      v-model="item.description"
                      filled
                      autogrow
                      bg-color="white"
                      label="Описание"
                      class="q-mb-md"
                    />
                    <q-input
                      v-model="item.item"
                      filled
                      autogrow
                      bg-color="white"
                      label="Товар"
                      class="q-mb-md"
                    />
                    <q-input
                      v-model="item.personName"
                      filled
                      autogrow
                      bg-color="white"
                      label="Продавец"
                      class="q-mb-md"
                    />
                    <q-input
                      v-model="item.personNumber"
                      filled
                      autogrow
                      bg-color="white"
                      label="Номер продавца"
                      class="q-mb-md"
                    />
                    <q-input
                      v-model="item.price"
                      filled
                      bg-color="white"
                      label="Цена"
                      class="q-mb-md"
                    />
                  </div>
                  <div class="image-container">
                    <img
                      :src="item.imageUrl"
                      alt="Item Picture"
                      class="center-image"
                    />
                  </div>
                </div>
              </div>
              <div
                class="q-mb-md"
                style="display: flex; justify-content: center"
              >
                <q-btn
                  @click="deleteShopItem(index, item)"
                  push
                  color="negative"
                  label="Удалить"
                  style="width: 25%"
                />
              </div>
            </div>
          </div>
        </div>
      </router-view>
    </q-page-container>
  </q-layout>
</template>

<script>
import { ref, onMounted } from "vue";
import {
  getStorage,
  ref as storageRef,
  getDownloadURL,
} from "firebase/storage";

export default {
  setup() {
    // Initialize an array to store shop items
    const shopItems = ref([]);

    // Initialize Firebase Storage
    const storage = getStorage();

    // Method to delete a shop item
    const deleteShopItem = (index, item) => {
      shopItems.value.splice(index, 1);

      const { itemId } = item;

      fetch(`http://localhost:5111/ShopItem/Delete`, {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify({ itemId }),
      })
        .then((response) => {
          if (!response.ok) {
            throw new Error(
              `Network response was not ok. Status: ${response.status}`
            );
          }
          return response.json();
        })
        .then((data) => {
          console.log("Success:", data);
          alert(data.message); // Show success message
        })
        .catch((error) => {
          console.error("Error:", error.message);
          alert("Failed to delete shop item. Please try again later.");
        });
    };

    // Method to fetch shop items from the server
    const fetchShopItems = async () => {
      try {
        const response = await fetch(
          "http://localhost:5111/ShopItem/GetShopItems"
        );
        const data = await response.json();

        // Load images for each shop item
        await Promise.all(
          data.map(async (item) => {
            item.imageUrl = await getImageURL(item.picture);
          })
        );

        // Set shop items after all images are loaded
        shopItems.value = data;
      } catch (error) {
        console.error("Error fetching shop items:", error);
      }
    };

    // Method to get the download URL for an image from Firebase Storage
    const getImageURL = async (imageName) => {
      const imageRef = storageRef(storage, `images/${imageName}.jpg`);
      try {
        const url = await getDownloadURL(imageRef);
        console.log("Image URL:", url); // Log the image URL before loading
        return url;
      } catch (error) {
        console.error("Error getting image URL:", error);
        return null;
      }
    };

    // Method to load all images and wait for their loading to complete
    const loadImages = async () => {
      const promises = shopItems.value.map(async (item) => {
        item.imageUrl = await getImageURL(item.picture);
        return new Promise((resolve, reject) => {
          const img = new Image();
          img.src = item.imageUrl;
          img.onload = resolve;
          img.onerror = reject;
        });
      });

      try {
        await Promise.all(promises);
        console.log("All images loaded successfully.");
      } catch (error) {
        console.error("Error loading images:", error);
      }
    };

    // Fetch shop items when component is mounted
    onMounted(async () => {
      await fetchShopItems();
      await loadImages();
    });

    return {
      shopItems,
      deleteShopItem,
    };
  },
};
</script>

<style>
.my-header {
  width: 100%;
  z-index: 1000;
}

.input-container {
  margin-top: 20px;
  margin-right: 20px;
  margin-left: 20px;
  background-color: lightgray;
  padding: 16px;
  border-radius: 16px;
}

.input-and-image {
  display: flex;
  align-items: center;
  width: 100%;
}

.input-fields {
  flex: 7;
}

.image-container {
  flex: 4;
  margin-left: 20px;
}

.center-image {
  max-width: 100%;
  height: auto;
}
</style>
