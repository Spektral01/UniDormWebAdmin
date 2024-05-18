import store from "../../store";

export function authGuard(to, from, next) {
  if (to.matched.some((record) => record.meta.requiresAuth)) {
    if (!store.getters.isAuthenticated) {
      next("/login");
    } else {
      next();
    }
  } else {
    next();
  }
}
