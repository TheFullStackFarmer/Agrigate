# Utilizing the Broker

The [EMQX](https://www.emqx.io/) MQTT broker is the hub of the IoT service.
Devices connect to the broker, which can then be managed from within Agrigate.

## Connecting

After starting Agrigate, you can connect to the broker by navigating to
http://localhost:18083. From there, you can log in with the default username /
password of admin / public. Once logging in, you'll be prompted to reset the
password.

Once logged in, you'll have access to a dashboard that allows you to view
connected devices, topics, subscriptions, and more

![dashboard](/img/guides/iot/mqtt-dashboard.PNG)

## Update Connection Rules

Agrigate uses the default EMQX ACL file with a modification that allows the
IoT Service's Device Broker to subscribe to system events. This can be changed
via the `/dc/message-broker/etc/acl.conf` file, or by navigating to the
Authorization settings within the MQTT Dashboard

![authorization](/img/guides/iot/mqtt-authorization.PNG)

## Connect a Device

To connect a device, you can use any MQTT library and connect to `{SERVER_IP}`
on port 1883. For testing purposes, you can utilize a client like
[MQTTX](https://mqttx.app/) and create a new connection

![connect](/img/guides/iot/mqttx-new-device.PNG)

For every device connected, there will be two connections in the MQTT broker -
one for the device, and an `{agrigate-{deviceId}}` which is the device actor
responsible for managing that device within Agrigate.

![devices](/img/guides/iot/mqtt-devices.PNG)
