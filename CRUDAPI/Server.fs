module Server

open Suave
open Suave.Successful
open Suave.Filters
open Suave.Operators
open Suave.Http.HttpRequest
open System.Web     
open Newtonsoft
open System.Text
open System

[<CLIMutable>]
type lancamento = {CentroId:Guid; Valor:decimal; Descricao:string}

let parseLanc input : lancamento =
    Json.JsonConvert.DeserializeObject<lancamento>(input)

let ``open`` () =
    startWebServer defaultConfig (choose [ pathScan "/insert/centroDeCusto/%s/%s" (fun (nome, codigo) -> let q = HttpUtility.UrlDecode(nome)
                                                                                                         let a = HttpUtility.UrlDecode(codigo)
                                                                                                         Persistence.insertCentroDeCusto q a
                                                                                                         OK (nome  + "_" + codigo))

                                           path "/insert/lancamento" >=> POST >=> request(fun r -> let stringContent = Encoding.UTF8.GetString(r.rawForm)
                                                                                                   let lanc = parseLanc stringContent
                                                                                                   Persistence.insertLancamento lanc.CentroId lanc.Valor lanc.Descricao
                                                                                                   OK "teste" ) ])