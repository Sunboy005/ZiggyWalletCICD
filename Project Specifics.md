### Ziggy Wallet Project Specifics

Build an e-wallet system that can be used by three types of users, “Noob”, “Elite” and “Admin”. The system is only accessible to authenticated users.

1. User (Noob) 
- is only entitled to one currency (main) on signup 
- All wallet funding must first be converted to main currency 
- All wallet withdrawals must first be converted to main currency 
- Cannot change the main currency.

2. User (Elite) 
- Can have multiple wallets in different currencies with the main currency selected at signup. 
- Funding in a particular currency should update the wallet with that currency or create it. 
- Withdrawals in a currency with funds in the wallet of that currency should reduce the wallet balance for that currency. 
- Withdrawals in a currency without a wallet balance should be converted to the main currency and withdrawn. 
- Cannot change main currency 

3. User (Admin) 
- Cannot have a wallet. 
- Cannot withdraw funds from any wallet. 
- Can fund wallets for Noob or Elite users in any currency. 
- Can change the main currency of any user. 
- Can promote or demote Noobs or Elite users 
- User (System) 
- Sends confirmation mail to user on every successful transaction
