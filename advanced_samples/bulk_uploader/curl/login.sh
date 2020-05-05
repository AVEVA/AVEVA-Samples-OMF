token=$(curl \
      --request POST \
      --data client_id=95b9d45f-372b-416e-9f16-11d2ccb5cd96 \
      --data client_secret=3OO423AZtSA4gke9iqMgttJ2yS0eDMUT7SW5mduxg34= \
      --data grant_type=client_credentials \
      https://dat-b.osisoft.com/identity/connect/token | jq -r '.access_token')
echo "${token}"

