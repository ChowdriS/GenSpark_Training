Basic Networking Commands:
1. ping
ping google.com
ping google.com -c 4
ping google.com -i 4

ping 8.8.8.8
2. nslookup
3. ip
4. ifconfig (Deprecated but still in use)
Prefer ip over ifconfig.

Docs:
https://www.geeksforgeeks.org/umask-command-in-linux-with-examples/
https://dev.to/kcdchennai/useradd-vs-adduser-which-command-to-use-3mb4
https://www.geeksforgeeks.org/linux-unix/ping-command-in-linux-with-examples/
https://www.geeksforgeeks.org/linux-unix/nslookup-command-in-linux-with-examples/
https://www.geeksforgeeks.org/linux-unix/ip-command-in-linux-with-examples/
https://www.geeksforgeeks.org/linux-unix/ifconfig-command-in-linux-with-examples/

Connectivity Testing:
1. curl: Fetchs web content
Transfers data from or to a server using various protocols (HTTP, FTP, etc.).
https://www.geeksforgeeks.org/linux-unix/curl-command-in-linux-with-examples/

2. wget: Downloads file
https://www.geeksforgeeks.org/linux-unix/wget-command-in-linux-unix/
Difference from curl:
wget is simpler and better for downloading files.
curl is more flexible for APIs and complex interactions.

3. traceroute
Traces the route packets take to reach a host.
Shows all the hops a packet takes from your machine to the target.
https://www.geeksforgeeks.org/linux-unix/traceroute-command-in-linux-with-examples/

4. netstat: Displays network connections, routing tables, interface statistics.
Note: ss is a faster modern alternative to netstat.
https://www.geeksforgeeks.org/linux-unix/ss-command-in-linux/

Linux File System Structure:
https://www.geeksforgeeks.org/linux-unix/linux-file-hierarchy-structure/

Package Management:
1. apt
2. yum
3. dnf
Doc for 1, 2, 3:
https://www.geeksforgeeks.org/linux-unix/understanding-package-managers-and-systemctl/

4. rpm
https://www.geeksforgeeks.org/linux-unix/how-to-use-the-rpm-command-in-linux/


