import Card from "../UI/Card"
const IotSensorDisplay = (props) =>{
return (
    <Card style={{ textAlign:'center' }}>
        <h1>Temperature</h1>
        <h2>{props.value} °C</h2>
    </Card>
)

}
export default IotSensorDisplay