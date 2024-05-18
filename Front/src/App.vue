<template>
  <div id="app">
    <router-view />
  </div>
</template>

<script>
import { createApp } from "vue";
import { createRouter, createWebHistory } from "vue-router";
import NotificationPage from "./pages/NotificationPage.vue"; // Импортируем вашу страницу уведомлений

const routes = [
  {
    path: "/login",
    component: NotificationPage, // Уведомления будут отображаться по умолчанию на главной странице
  },
];

const router = createRouter({
  history: createWebHistory(),
  routes,
});

createApp({
  router,
}).mount("#app");
</script>
