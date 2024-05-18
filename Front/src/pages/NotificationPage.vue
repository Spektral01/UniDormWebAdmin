<template>
  <q-layout view="hHh lpR fFf">
    <q-page-container>
      <router-view>
        <div class="content">
          <div class="my-header">
            <q-header class="bg-primary text-white" height-hint="98" fixed>
              <!-- Add 'fixed' attribute to make the header fixed -->
              <q-toolbar>
                <q-toolbar-title>
                  <q-avatar>
                    <img
                      src="https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcRqJhV0BsZQGBGfxK-8EHS05JF4vdeYvN204NHOLOMg4A&s"
                    />
                  </q-avatar>
                  Объявления
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

          <div class="add-container">
            <q-btn
              push
              color="primary"
              label="Добавить объявление"
              @click="addContainer"
            />
          </div>

          <div
            v-for="(container, index) in containers"
            :key="index"
            class="input-container"
          >
            <div class="q-pa-md row-horizontal-alignment">
              <div class="row justify-between">
                <div class="input">
                  <q-input
                    v-model="container.lbl"
                    filled
                    autogrow
                    label="Заголовок"
                    bg-color="white"
                  />
                </div>

                <div class="buttons">
                  <q-btn
                    push
                    color="primary"
                    label="Загрузить"
                    @click="pushToServer(container)"
                    class="push-button"
                  />
                  <q-btn
                    @click="deleteContainer(index, container)"
                    push
                    color="negative"
                    label="Удалить"
                  />
                </div>
              </div>
            </div>
            <div class="q-pa-md inputtextarea">
              <q-input
                v-model="container.text"
                filled
                autogrow
                bg-color="white"
                label="Текст"
              />
            </div>
          </div>
        </div>
      </router-view>
    </q-page-container>
  </q-layout>
</template>

<script>
import { ref, onMounted } from "vue";

export default {
  setup() {
    // Structure to store notification data
    const notificationMap = ref({});

    // Initialize an array to store container data
    const containers = ref([]);

    // Method to add a new container
    const addContainer = () => {
      containers.value.push({
        lbl: "", // Default text for new container
        text: "",
        notifID: "", // Empty notifID for new container
      });
    };

    // Method to delete a container
    const deleteContainer = (index, container) => {
      containers.value.splice(index, 1);

      const { lbl: text, notifID } = container;

      fetch(`http://localhost:5111/Notification/Delete`, {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify({ notifText: text, notifID }),
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
          alert("Failed to update notification. Please try again later.");
        });
    };

    // Method to send text to ASP.NET server
    const pushToServer = (container) => {
      const { lbl: notifTitle, text: notifText, notifID } = container;

      if (notifID !== "") {
        // Если notifID существует, отправляем запрос на обновление существующего контейнера
        fetch(`http://localhost:5111/Notification/Edit`, {
          method: "POST",
          headers: {
            "Content-Type": "application/json",
          },
          body: JSON.stringify({ notifTitle, notifText, notifID }), // измените поле на notifTitle
        })
          .then((response) => {
            if (!response.ok) {
              throw new Error(
                `Сетевой ответ не был успешным. Статус: ${response.status}`
              );
            }
            return response.json();
          })
          .then((data) => {
            console.log("Успешно:", data);
            alert(data.message); // Показать сообщение об успешном выполнении
          })
          .catch((error) => {
            console.error("Ошибка:", error.message);
            alert(
              "Не удалось обновить уведомление. Пожалуйста, попробуйте еще раз позже."
            );
          });
      } else {
        // Если notifID не существует, отправляем запрос на создание нового контейнера
        fetch("http://localhost:5111/Notification/Create", {
          method: "POST",
          headers: {
            "Content-Type": "application/json",
          },
          body: JSON.stringify({ notifTitle, notifText, notifID: "" }), // измените поле на notifTitle
        })
          .then((response) => {
            if (!response.ok) {
              throw new Error(
                `Сетевой ответ не был успешным. Статус: ${response.status}`
              );
            }
            return response.json();
          })
          .then((data) => {
            console.log("Успешно:", data);
            // Обработать успешный ответ при необходимости
          })
          .catch((error) => {
            console.error("Ошибка:", error.message);
            // Отобразить понятное для пользователя сообщение об ошибке
            alert(
              "Не удалось отправить уведомление. Пожалуйста, попробуйте еще раз позже."
            );
          });
      }
    };

    // Method to fetch notification data from server
    const fetchNotificationData = () => {
      fetch("http://localhost:5111/Notification/GetNotif")
        .then((response) => response.json())
        .then((data) => {
          // Clear previous data
          containers.value = [];
          // Fill containers array with data from database
          data.forEach((item) => {
            containers.value.push({
              lbl: item.notifTitle, // Use notifLabel from database
              text: item.notifText, // Use notifText from database
              notifID: item.notifID,
            });
          });
        })
        .catch((error) => {
          console.error("Error fetching notification data:", error);
        });
    };

    // Fetch notification data when component is mounted
    onMounted(() => {
      fetchNotificationData();
    });

    return {
      containers,
      addContainer,
      deleteContainer,
      pushToServer,
    };
  },
};
</script>

<style>
.my-header {
  width: 100%;
  z-index: 1000; /* Ensure the header stays on top of other content */
}

.input-container {
  margin-top: 20px;
  margin-right: 20px;
  margin-left: 20px;
  background-color: lightgray;
  padding: 16px;
  border-radius: 16px;
}

.input {
  width: 1000px;
}

.buttons {
  height: 30px;
}

.push-button {
  margin-right: 10px;
}

.add-container {
  margin-top: 20px;
  margin-left: 20px;
}

.inputtextarea {
  max-width: 100%;
}
</style>
