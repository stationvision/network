//using Monitoring.Db.Extensions;
//using Monitoring.Db.Interfaces;
//using Monitoring.Db.Models;
//using NetworkCommunications.Dtos;
//using NetworkCommunications.Extentions;
//using NetworkCommunications.MethodTypes;

//namespace NetworkCommunications.Services
//{
//    public class AggregationService
//    {
//        public AggregationService(IServiceProvider serviceProvider)
//        {
//            _serviceProvider = serviceProvider;
//        }
//        private static readonly object _lockObject = new object();
//        //public async Task AggregateMessages(MessageBuffer buffer, CancellationToken cancellationToken)
//        public void AggregateMessages(MessageEntity buffermessages, CancellationToken cancellationToken)
//        {
//            Console.WriteLine(buffermessages.Content);
//            int TotalMessageReceived = 0;
//            using (var scope = _serviceProvider.CreateScope())
//            {
//                var messageEntityRepository = scope.ServiceProvider.GetRequiredService<IRepository<MessageEntity>>();
//                var pulsRepository = scope.ServiceProvider.GetRequiredService<IRepository<Puls>>();
//                var clientpulsRepository = scope.ServiceProvider.GetRequiredService<IRepository<ClientPuls>>();
//                var _requireDataDto = new RequireDataDto();

//                //while (buffer.TryGetNextMessage(out var buffermessages))

//                //foreach (var buffermessages in buffer.GetMessage())
//                {
//                    TotalMessageReceived++;
//                    //Console.WriteLine($"Processing message {TotalMessageReceived}...");
//                    //if (cancellationToken.IsCancellationRequested) break;
//                    try
//                    {
//                        Monitor.Enter(_lockObject);
//                        {

//                            var buffermessagesJson = buffermessages.Content.ToJson();
//                            var MessagePulses = BindClientPuls(buffermessages, pulsRepository, clientpulsRepository);

//                            MessagePulses = MessagePulses.OrderBy(x => x.puls.Name).OrderBy(x => x.puls.Name.Length).ToList();
//                            foreach (var Newpuls in MessagePulses) // does the process for each puls ADC0 - ADC16
//                            {

//                                if (long.Parse(Newpuls.Value) >= 4000)
//                                {
//                                    var ssss = Newpuls;

//                                }
//                                if (Newpuls.puls.Name == "ADC2")
//                                {
//                                    var ssss = Newpuls;
//                                }
//                                if (long.Parse(Newpuls.Value) >= 4000)
//                                {
//                                    var ssassss = Newpuls;

//                                }
//                                var latestPuls = clientpulsRepository.Query(x => x.BoardId == buffermessages.BoardId && x.PulsId == Newpuls.PulsId).IncludeMultiple(i => i.Board, i => i.puls).OrderByDescending(x => x.Timestamp).FirstOrDefault();

//                                var newPulsRange = new ClientToServerDto().GetRangeName(Newpuls);

//                                if (latestPuls is null)
//                                {
//                                    //var deSeriilizeLatestMessage = JsonSerializer.Deserialize<ClientToServerDto>(newPuls.Data);

//                                    //means this is first message thats coming from the board
//                                    CreateNewClientPulsRecord(Newpuls, clientpulsRepository);
//                                    continue;
//                                }
//                                var latestPulsRange = new ClientToServerDto().GetRangeName(latestPuls);



//                                bool NewMessageIsDifferent = IsNewMessageDifferentfromLatestMessage(latestPuls, Newpuls);

//                                if (!NewMessageIsDifferent)
//                                {
//                                    //Update OldMessage Start Date
//                                    UpdateLatestClientPulsStartDate(latestPuls, clientpulsRepository);
//                                    EmptyDeviationTime(_requireDataDto);
//                                    ChangeIncreaseProductionTime(false, _requireDataDto);
//                                    continue;
//                                }

//                                {

//                                    if (latestPulsRange.RangeName == newPulsRange.RangeName)
//                                    {
//                                        //Yes, then check for
//                                        //Is the difference between the current number and the previous number less than the numerical difference that can be ignored?
//                                        if (Math.Abs(latestPuls.Value.ToDecimal().Value - Newpuls.Value.ToDecimal().Value) <= latestPuls.puls.IgnoredDifferenceThreshold)
//                                        {
//                                            // If yes, then update OldMessage Start Date
//                                            UpdateLatestClientPulsStartDate(latestPuls, clientpulsRepository);
//                                            EmptyDeviationTime(_requireDataDto);
//                                            ChangeIncreaseProductionTime(false, _requireDataDto);
//                                            continue;
//                                        }
//                                        else
//                                        {
//                                            if (!_requireDataDto.DeviationTime.HasValue) //Thats new
//                                            {
//                                                SetPulsDateToDeviationTime(Newpuls, _requireDataDto);
//                                            }
//                                            IFDeviationDiffrenceLessThanIgnorableduration(Newpuls, latestPuls, clientpulsRepository, newPulsRange, latestPulsRange, _requireDataDto);
//                                            continue;
//                                        }
//                                    }
//                                    else
//                                    {
//                                        if (!_requireDataDto.DeviationTime.HasValue) //Thats new
//                                        {
//                                            SetPulsDateToDeviationTime(Newpuls, _requireDataDto);
//                                        }
//                                        IFDeviationDiffrenceLessThanIgnorableduration(Newpuls, latestPuls, clientpulsRepository, newPulsRange, latestPulsRange, _requireDataDto);
//                                        continue;
//                                    }

//                                }


//                            }

//                        }
//                    }
//                    catch (Exception ex)
//                    {
//                        Console.WriteLine(ex.Message);
//                    }
//                    finally
//                    {
//                        Monitor.Exit(_lockObject);
//                        Console.WriteLine($"Message {TotalMessageReceived} processed.");
//                        //CleanBuffer(buffer.MessageId);
//                    }
//                }
//            }
//        }

//        private List<ClientPuls> BindClientPuls(MessageEntity NewMessage, IRepository<Puls> PulsRepository, IRepository<ClientPuls> ClientPulsRepository)
//        {

//            var result = new List<ClientPuls>();
//            string[] splitArray = NewMessage.Content.Split(',');
//            List<string> incommingData = new List<string>(splitArray);


//            var Pulsdatas = PulsRepository.Query(x => x.BoardId == NewMessage.BoardId).OrderBy(x => x.Name).OrderBy(x => x.Name.Length).ToList();
//            string PulsNamePrefix = "ADC";

//            foreach (var item in Pulsdatas)
//            {

//                if (item.Name == PulsNamePrefix + 0)
//                {
//                    var res = CreateData(NewMessage, item, incommingData[2], incommingData);
//                    result.Add(res);
//                    continue;
//                }
//                if (item.Name == PulsNamePrefix + 1)
//                {
//                    var res = CreateData(NewMessage, item, incommingData[3], incommingData);
//                    result.Add(res);
//                    continue;
//                }
//                if (item.Name == PulsNamePrefix + 2)
//                {
//                    var res = CreateData(NewMessage, item, incommingData[4], incommingData);
//                    result.Add(res);
//                    continue;
//                }
//                if (item.Name == PulsNamePrefix + 3)
//                {
//                    var res = CreateData(NewMessage, item, incommingData[5], incommingData);
//                    result.Add(res);
//                    continue;
//                }
//                if (item.Name == PulsNamePrefix + 4)
//                {
//                    var res = CreateData(NewMessage, item, incommingData[6], incommingData);
//                    result.Add(res);
//                    continue;
//                }
//                if (item.Name == PulsNamePrefix + 5)
//                {
//                    var res = CreateData(NewMessage, item, incommingData[7], incommingData);
//                    result.Add(res);
//                    continue;
//                }
//                if (item.Name == PulsNamePrefix + 6)
//                {
//                    var res = CreateData(NewMessage, item, incommingData[8], incommingData);
//                    result.Add(res);
//                    continue;
//                }
//                if (item.Name == PulsNamePrefix + 7)
//                {
//                    var res = CreateData(NewMessage, item, incommingData[9], incommingData);
//                    result.Add(res);
//                    continue;
//                }
//                if (item.Name == PulsNamePrefix + 8)
//                {
//                    var res = CreateData(NewMessage, item, incommingData[10], incommingData);
//                    result.Add(res);
//                    continue;
//                }
//                if (item.Name == PulsNamePrefix + 9)
//                {
//                    var res = CreateData(NewMessage, item, incommingData[11], incommingData);
//                    result.Add(res);
//                    continue;
//                }
//                if (item.Name == PulsNamePrefix + 10)
//                {
//                    var res = CreateData(NewMessage, item, incommingData[12], incommingData);
//                    result.Add(res);
//                    continue;
//                }
//                if (item.Name == PulsNamePrefix + 11)
//                {
//                    var res = CreateData(NewMessage, item, incommingData[13], incommingData);
//                    result.Add(res);
//                    continue;
//                }
//                if (item.Name == PulsNamePrefix + 12)
//                {
//                    var res = CreateData(NewMessage, item, incommingData[14], incommingData);
//                    result.Add(res);
//                    continue;
//                }
//                if (item.Name == PulsNamePrefix + 13)
//                {
//                    var res = CreateData(NewMessage, item, incommingData[15], incommingData);
//                    result.Add(res);
//                    continue;
//                }
//                if (item.Name == PulsNamePrefix + 14)
//                {
//                    var res = CreateData(NewMessage, item, incommingData[16], incommingData);
//                    result.Add(res);
//                    continue;
//                }
//                if (item.Name == PulsNamePrefix + 15)
//                {
//                    var res = CreateData(NewMessage, item, incommingData[17], incommingData);
//                    result.Add(res);
//                    continue;
//                }
//                if (item.Name == PulsNamePrefix + 16)
//                {
//                    var res = CreateData(NewMessage, item, incommingData[18], incommingData);
//                    result.Add(res);
//                    continue;
//                }
//            }
//            return result;
//        }

//        private ClientPuls CreateData(MessageEntity newMessage, Puls item, string value, List<string> incommingData)
//        {
//            var clientPuls = new ClientPuls()
//            {
//                puls = item,
//                PulsId = item.Id,
//                Value = value,
//                Count = 0,
//                BoardId = newMessage.BoardId,
//                //MachineId =item.MachineId,TODO: Add MachineId to Puls
//                Timestamp = DateTime.Now,
//                StartDate = (incommingData[22] + "," + incommingData[23]).ToDateTime(),
//                Hash = value.ComputeSha256Hash(),
//            };
//            return clientPuls;
//        }

//        private void CleanBuffer(Guid messageId)
//        {
//            // _messageBuffers.TryRemove(messageId, out _);
//        }

//        public bool testing = false;
//        private readonly IServiceProvider _serviceProvider;

//        private void SetPulsDateToDeviationTime(ClientPuls newPuls, RequireDataDto requireDataDto)
//        {
//            var dateTime = newPuls.StartDate; // Replace with your actual DateTime object
//            DateTime now = DateTime.Now;
//            TimeSpan timeSpan = now - dateTime.Value;
//            double totalSeconds = timeSpan.TotalSeconds;
//            requireDataDto.DeviationTime = totalSeconds;
//        }


//        private void IFDeviationDiffrenceLessThanIgnorableduration(ClientPuls newPuls, ClientPuls latestClientPuls,
//            IRepository<ClientPuls> clientPulsRepository, GetRangeNameResultItem newPulsRange, GetRangeNameResultItem latestPulsRange, RequireDataDto requireDataDto)
//        {
//            var dateTime = newPuls.StartDate.Value;
//            DateTime now = DateTime.Now;
//            TimeSpan timeSpan = now - dateTime;
//            double totalSeconds = timeSpan.TotalSeconds;


//            if (requireDataDto.DeviationTime < latestClientPuls.puls.IgnoredDurationThreshold)
//            {
//                UpdateLatestClientPulsStartDate(latestClientPuls, clientPulsRepository);
//                if (!requireDataDto.IncreaseProductionTime)
//                {
//                    ChangeIncreaseProductionTime(true, requireDataDto);
//                    UpdateLatestClientPulsEndDateAndIncreaseCount(latestClientPuls, clientPulsRepository);
//                    return;
//                }
//                return;

//            }
//            else
//            {
//                CommitLatestClientPuls(latestClientPuls, clientPulsRepository);
//                CreateNewClientPulsRecord(newPuls, clientPulsRepository);
//                return;

//            }
//        }
//        private void EmptyDeviationTime(RequireDataDto requireDataDto)
//        {
//            requireDataDto.DeviationTime = null;
//        }
//        private void ChangeIncreaseProductionTime(bool status, RequireDataDto requireDataDto)
//        {
//            requireDataDto.IncreaseProductionTime = status;
//        }
//        private void CreateNewClientPulsRecord(ClientPuls newpuls, IRepository<ClientPuls> clientPulsrepository)
//        {

//            clientPulsrepository.Add(newpuls);
//            clientPulsrepository.SaveChanges();

//        }
//        private void CommitLatestClientPuls(ClientPuls latestPuls, IRepository<ClientPuls> Repository)
//        {
//            if (latestPuls == null)
//                return;
//            latestPuls.EndDate = DateTime.Now;
//            Repository.Update(latestPuls);
//            Repository.SaveChanges();
//        }
//        private void UpdateLatestClientPulsEndDateAndIncreaseCount(ClientPuls latestPuls, IRepository<ClientPuls> repository)
//        {
//            latestPuls.EndDate = DateTime.Now;
//            int c = latestPuls.Count;
//            c++;
//            latestPuls.Count = c;
//            repository.Update(latestPuls);
//            repository.SaveChanges();
//        }
//        private bool IsNewMessageDifferentfromLatestMessage(ClientPuls? latestPuls, ClientPuls newPuls)
//        {
//            if (latestPuls is null) //Step2: Is new message different from the old message?
//                return true;

//            if (latestPuls.Hash != newPuls.Hash)
//                return true;

//            return false;

//        }
//        private void UpdateLatestClientPulsStartDate(ClientPuls oldMessage, IRepository<ClientPuls> repository)
//        {
//            oldMessage.EndDate = DateTime.Now;
//            repository.Update(oldMessage);
//            repository.SaveChanges();
//        }

//    }
//}
