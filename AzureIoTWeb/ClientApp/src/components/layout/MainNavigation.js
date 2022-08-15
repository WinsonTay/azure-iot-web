//import { NavLink } from "react-router-dom";
import classes from "./MainNavigation.module.css";
const MainNavigation = () => {
  return (
    <header className={classes.header}>
      <div className={classes.logo}>Azure IoT </div>
      <nav className={classes.nav}>
        <ul>
        </ul>
      </nav>
    </header>
  );
};
export default MainNavigation;
