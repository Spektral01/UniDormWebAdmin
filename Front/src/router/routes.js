import NotificationPage from "pages/NotificationPage.vue";
import LoginPage from "pages/LoginPage.vue";
import ShopPage from "pages/ShopPage.vue";
import DormitoryPage from "pages/DormitoryPage.vue";
import UserPage from "pages/UserPage.vue";
import { authGuard } from "./guards/authGuard.js";

const routes = [
  {
    path: "",
    redirect: "/login", // Перенаправление на страницу логина
  },
  {
    path: "/login",
    component: LoginPage,
  },
  {
    path: "/notification",
    component: NotificationPage,
    meta: {
      requiresAuth: true,
    },
    beforeEnter: authGuard,
  },
  {
    path: "/shop",
    component: ShopPage,
    meta: {
      requiresAuth: true,
    },
    beforeEnter: authGuard,
  },
  {
    path: "/dormitory",
    component: DormitoryPage,
    meta: {
      requiresAuth: true,
    },
    beforeEnter: authGuard,
  },
  {
    path: "/user",
    component: UserPage,
    meta: {
      requiresAuth: true,
    },
    beforeEnter: authGuard,
  },
];

export default routes;
