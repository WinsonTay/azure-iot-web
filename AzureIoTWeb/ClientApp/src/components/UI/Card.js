import classes from './Card.module.css';

const Card = (props) => {
  return <div style={{textAlign:"center"}} className={classes.card}>{props.children}</div>;
};

export default Card;
