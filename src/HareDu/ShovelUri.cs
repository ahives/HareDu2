// Copyright 2013-2018 Albert L. Hives
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
namespace HareDu
{
    public interface ShovelUri
    {
        void SetUsername(string username);

        void SetPassword(string password);

        void SetHost(string host);

        void SetPort(int port);

        void SetCertificateAuthority(string certificateAuthority);

        void SetCertificateFile(string certificateFile);

        void SetKeyFile(string keyFile);

        void SetSaslAuthenticationMechanism(string saslAuthMechanism);
    }
}