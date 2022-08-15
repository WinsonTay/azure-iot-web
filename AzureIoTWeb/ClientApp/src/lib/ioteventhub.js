// Copyright (c) Microsoft Corporation.
// Licensed under the MIT Licence.

/**
 * @summary Demonstrates how to use the EventHubConsumerClient to process events from all partitions of a consumer group in an Event Hub.
 */

 import { EventHubConsumerClient, latestEventPosition } from "@azure/event-hubs"


 
 const connectionString = "Endpoint=sb://ihsuprodsgres026dednamespace.servicebus.windows.net/;SharedAccessKeyName=iothubowner;SharedAccessKey=rzHAwoGsTb6fVwWyXIbaZzIqnpKRvITSs6HzAehJQpI=;EntityPath=iothub-ehub-winsonioth-20611456-ec47ae0394"
 const eventHubName = "iothub-ehub-winsonioth-20611456-ec47ae0394"
 const consumerGroup = "$Default";
 
 export async function startListenEvent() {
   console.log(`Start Listening Azure IoT Event... `);
 
   const consumerClient = new EventHubConsumerClient(consumerGroup, connectionString, eventHubName);
   const partitionIds = await consumerClient.getPartitionIds();
 
    consumerClient.subscribe(
     partitionIds[0],
     {
       // The callback where you add your code to process incoming events
       processEvents: async (events, context) => {
         // Note: It is possible for `events` to be an empty array.
         // This can happen if there were no new events to receive
         // in the `maxWaitTimeInSeconds`, which is defaulted to
         // 60 seconds.
         // The `maxWaitTimeInSeconds` can be changed by setting
         // it in the `options` passed to `subscribe()`.              
         for (const event of events) {
           //console.log(event.body.Content)
           const telemetryData = event.body.Content[0]
           const telemetryValue = telemetryData.Data[0].Values[0]
           console.log(telemetryValue)
           //console.log(
             //`Received event: '${event.body.Content[0]}' from partition: '${context.partitionId}' and consumer group: '${context.consumerGroup}'`
           //);
         }
       },
       processError: async (err, context) => {
         console.log(`Error on partition "${context.partitionId}": ${err}`);
       },
     },
     { startPosition: latestEventPosition }
   );
 
   // Wait for a bit before cleaning up the sample
  //  setTimeout(async () => {
  //    await subscription.close();
  //    await consumerClient.close();
  //    console.log(`Exiting receiveEvents sample`);
  //  }, 30 * 1000);
 }
 
//  startListenEvent().catch((error) => {
//    console.error("Error running sample:", error);
//  });
 
//  export default startListenEvent