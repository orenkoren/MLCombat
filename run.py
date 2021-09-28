import torch
import time

dev = torch.device("cuda") if torch.cuda.is_available() else torch.device("cpu")
t1 = torch.randn(10000,1500)
t2 = torch.randn(1500)


t1 = t1.to(dev)
t2 = t2.to(dev)

print(t1.is_cuda) 
print(t2.is_cuda) 


s = time.time()
for i in range(0, 10000):
    t3 = torch.matmul(t1, t2)
e = time.time()
print(e-s)