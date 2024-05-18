<template>
  <div class="content">
    <q-layout view="hHh lpR fFf">
      <q-page-container>
        <q-header class="bg-primary text-white" height-hint="98" fixed>
          <q-toolbar>
            <q-toolbar-title>
              <q-avatar>
                <img
                  src="https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcRqJhV0BsZQGBGfxK-8EHS05JF4vdeYvN204NHOLOMg4A&s"
                />
              </q-avatar>
              Общежития
            </q-toolbar-title>
          </q-toolbar>

          <q-tabs align="left">
            <q-route-tab to="/notification" label="Объявления" />
            <q-route-tab to="/shop" label="Магазин" />
            <q-route-tab to="/dormitory" label="Общежития" />
            <q-route-tab to="/user" label="Студенты" />
          </q-tabs>
        </q-header>

        <div class="add-container">
          <q-btn
            push
            color="primary"
            label="Добавить общежитие"
            @click="addDormitory"
          />
        </div>

        <div
          v-for="(dormitory, index) in dormitories"
          :key="index"
          class="input-container"
        >
          <div class="q-pa-md row-horizontal-alignment">
            <div class="row justify-between">
              <div class="input-and-image">
                <div class="input-fields">
                  <q-input
                    v-model="dormitory.dormName"
                    filled
                    bg-color="white"
                    label="Общежитие"
                    class="q-mb-md"
                  />
                  <q-input
                    v-model="dormitory.dormAddress"
                    filled
                    bg-color="white"
                    label="Адрес"
                    class="q-mb-md"
                  />
                  <q-input
                    v-model="dormitory.castellanTime"
                    filled
                    bg-color="white"
                    label="Castellan Time"
                    class="q-mb-md"
                  />
                  <q-input
                    v-model="dormitory.commendTime"
                    filled
                    bg-color="white"
                    label="Commend Time"
                    class="q-mb-md"
                  />
                  <q-input
                    v-model="dormitory.passportTime"
                    filled
                    bg-color="white"
                    label="Passport Time"
                    class="q-mb-md"
                  />
                  <q-input
                    v-model="dormitory.showerTime"
                    filled
                    bg-color="white"
                    label="Shower Time"
                    class="q-mb-md"
                  />
                  <q-input
                    filled
                    bg-color="white"
                    label="Coords"
                    class="q-mb-md"
                  />
                </div>
                <div class="image-container">
                  <img
                    :src="dormitory.dormPhoto"
                    alt="Dormitory Photo"
                    class="center-image"
                  />
                  <div v-if="dormitory.showInput">
                    <q-input
                      v-model="dormitory.newImageUrl"
                      filled
                      bg-color="white"
                      label="Image URL"
                      class="q-mt-md"
                    />
                    <q-btn
                      @click="saveImage(dormitory)"
                      push
                      color="primary"
                      label="Сохранить"
                      style="margin-top: 10px"
                    />
                  </div>
                  <div
                    class="q-mb-md"
                    style="display: flex; justify-content: center"
                  >
                    <q-btn
                      @click="toggleInput(dormitory)"
                      push
                      color="primary"
                      label="Добавить картинку"
                      style="margin-top: 10px; width: 25%"
                    />
                  </div>
                </div>
              </div>
            </div>
            <div class="q-mb-md" style="display: flex; justify-content: center">
              <q-btn
                @click="pushToServer(dormitory)"
                push
                color="primary"
                label="Загрузить"
                style="width: 25%"
              />
            </div>
            <div class="q-mb-md" style="display: flex; justify-content: center">
              <q-btn
                @click="deleteDormitory(index, dormitory)"
                push
                color="negative"
                label="Удалить"
                style="width: 25%"
              />
            </div>
          </div>
        </div>
      </q-page-container>
    </q-layout>
  </div>
</template>

<script>
import { ref, onMounted } from "vue";

export default {
  setup() {
    // Initialize an array to store dormitories
    const dormitories = ref([]);

    // Method to fetch dormitories from the server
    const fetchDormitories = async () => {
      try {
        const response = await fetch(
          "http://localhost:5111/Dormitory/GetDormitories"
        );
        const data = await response.json();
        dormitories.value = data;
      } catch (error) {
        console.error("Error fetching dormitories:", error);
      }
    };

    // Method to delete a dormitory
    const deleteDormitory = (index, dormitory) => {
      dormitories.value.splice(index, 1);

      const { dormId } = dormitory;

      fetch(`http://localhost:5111/Dormitory/Delete`, {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify({ dormId }),
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
          alert("Failed to delete dormitory. Please try again later.");
        });
    };

    // Method to push a dormitory to the server
    const pushToServer = (dormitory) => {
      fetch("http://localhost:5111/Dormitory/Create", {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify(dormitory),
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
          alert("Failed to add dormitory. Please try again later.");
        });
    };

    // Fetch dormitories when component is mounted
    onMounted(async () => {
      await fetchDormitories();
    });

    const toggleInput = (dormitory) => {
      dormitory.showInput = !dormitory.showInput;
    };

    // Method to save the new image URL
    const saveImage = (dormitory) => {
      dormitory.dormPhoto = dormitory.newImageUrl;
      dormitory.showInput = false;
    };

    return {
      dormitories,
      deleteDormitory,
      pushToServer,
      toggleInput,
      saveImage,
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

.add-container {
  margin-top: 20px;
  margin-right: 20px;
  margin-left: 20px;
  display: flex;
  justify-content: flex-end;
}
</style>
