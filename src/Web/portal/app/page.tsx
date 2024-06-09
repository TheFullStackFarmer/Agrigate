import { Card, CardContent, Typography } from "@mui/joy";

export default function Dashboard() {
  return (
    <div className="flex flex-wrap justify-around gap-4">
      <Card variant="soft" className="min-w-80">
        <CardContent>
          <Typography>Weather!</Typography>
        </CardContent>
      </Card>
      <Card variant="soft" className="min-w-80">
        <CardContent>
          <Typography>Weekly Rainfall</Typography>
        </CardContent>
      </Card>
      <Card variant="soft" className="min-w-80">
        <CardContent>
          <Typography>Connected Devices</Typography>
        </CardContent>
      </Card>
    </div>
  );
}
